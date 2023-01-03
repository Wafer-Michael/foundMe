using System.Collections;
using System.Collections.Generic;
using UnityEngine;


//--------------------------------------------------------------------------------------
///	壁回避用の触覚
//--------------------------------------------------------------------------------------
public class WallAvoidTacticle
{
    float m_range;  //長さ
    float m_degree; //角度

    public WallAvoidTacticle() :
        this(45.0f)
    { }

    public WallAvoidTacticle(float degree) :
        this(5.0f, degree)
    { }

    public WallAvoidTacticle(float range, float degree)
    {
        m_range = range;
        m_degree = degree;
    }

    public float GetRange() { return m_range; }

    public float GetDegree() { return m_degree; }

    public float GetRad() { return Mathf.Deg2Rad * m_degree; }
}

//--------------------------------------------------------------------------------------
///	壁回避
//--------------------------------------------------------------------------------------
public class WallAvoid : MonoBehaviour
{
    public static readonly Parametor DEFAULT_PARAMETOR = new Parametor(5.0f, 0.75f, new Vector3(0.0f, -0.35f, 0.0f));

    //--------------------------------------------------------------------------------------
    ///	壁回避パラメータ
    //--------------------------------------------------------------------------------------
    [System.Serializable]
    public struct Parametor
    {
        public float avoidRange;
        public float avoidPower;
        public Vector3 offset;

        public Parametor(float avoidRange, float avoidPower, Vector3 offset)
        {
            this.avoidRange = avoidRange;
            this.avoidPower = avoidPower;
            this.offset = offset;
        }
    };

    [SerializeField]
    private Parametor m_param = DEFAULT_PARAMETOR;

    private Vector3 m_avoidVec = Vector3.zero;

    private List<WallAvoidTacticle> m_tacticles = new List<WallAvoidTacticle>();

    private void Start()
    {
        //触覚の生成
        SettingDefaultTacticles();
    }

    private void Update()
    {
        AvoidUpdate();

        Debug.Log("★" + TakeAvoidVector().ToString());
    }

    void SettingDefaultTacticles()
    {
        const float DEGREE_VALUE = 15.0f;
        const float DEGREE_VALUE_Side = 40.0f;
        float[] degrees = new float[]{
            +DEGREE_VALUE,
            -DEGREE_VALUE,
            +DEGREE_VALUE_Side,
            -DEGREE_VALUE_Side
        };

        //デフォで四つの触覚をはやす。
        foreach (float degree in degrees)
        {
            var tacticle = new WallAvoidTacticle(m_param.avoidRange, degree);
            m_tacticles.Add(tacticle);
        }
    }

    private void AvoidUpdate()
    {
        foreach (var tacticle in m_tacticles)
        {
            var avoidVec = CalculateAvoidVec(tacticle);
            if (avoidVec != Vector3.zero)
            {
                m_avoidVec = avoidVec;  //入力に力を加える。
                break;
            }
        }
    }

    private Vector3 CalculateAvoidVec(WallAvoidTacticle tacticle)
    {
        var result = Vector3.zero;
        var startPosition = transform.position + m_param.offset;
        var rayDirection = CalculateRayDirection(tacticle);
        var targetPosition = startPosition + rayDirection;

        //レイがヒットしたら、減衰距離を返す。
        if (maru.UtilityObstacle.IsRayObstacle(startPosition, targetPosition))
        {
            result += -rayDirection;
        }

        return result.normalized * m_param.avoidPower;
    }

    private Vector3 CalculateRayDirection(WallAvoidTacticle tacticle)
    {
        var forward = transform.forward;
        //var direction = maru::Mathf::RotationDegreeVec(forward, tacticle.GetDegree(), Vector3.up);
        var direction = Quaternion.AngleAxis(tacticle.GetRad(), Vector3.up) * forward;
        return direction.normalized * tacticle.GetRange();
    }

    public Vector3 TakeAvoidVector()
    {
        var result = m_avoidVec;
        m_avoidVec = Vector3.zero;
        return result;
    }

}
