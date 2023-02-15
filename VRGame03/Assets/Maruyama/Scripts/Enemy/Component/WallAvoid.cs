using System.Collections;
using System.Collections.Generic;
using UnityEngine;


//--------------------------------------------------------------------------------------
///	�ǉ��p�̐G�o
//--------------------------------------------------------------------------------------
public class WallAvoidTacticle
{
    float m_range;  //����
    float m_degree; //�p�x

    public WallAvoidTacticle() :
        this(45.0f)
    { }

    public WallAvoidTacticle(float degree) :
        this(2.0f, degree)
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
///	�ǉ��
//--------------------------------------------------------------------------------------
public class WallAvoid : MonoBehaviour
{
    public static readonly Parametor DEFAULT_PARAMETOR = new Parametor(2.0f, 0.1f, new Vector3(0.0f, -0.35f, 0.0f));

    //--------------------------------------------------------------------------------------
    ///	�ǉ���p�����[�^
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

    [SerializeField]
    private GameObject m_testerPrefab;

    private void Start()
    {
        //�G�o�̐���
        SettingDefaultTacticles();
    }

    private void Update()
    {
        AvoidUpdate();
    }

    private void SettingDefaultTacticles()
    {
        const float DEGREE_VALUE = 15.0f;
        const float DEGREE_VALUE_Side = 60.0f;
        float[] degrees = new float[]{
            +DEGREE_VALUE,
            -DEGREE_VALUE,
            +DEGREE_VALUE_Side,
            -DEGREE_VALUE_Side
        };

        //�f�t�H�Ŏl�̐G�o���͂₷�B
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
            if (avoidVec != Vector3.zero) {
                m_avoidVec = avoidVec;  //���͂ɗ͂�������B
                break;
            }
        }

        //Debug.Log("����" + m_avoidVec.ToString());
    }

    private Vector3 CalculateAvoidVec(WallAvoidTacticle tacticle)
    {
        var result = Vector3.zero;
        var startPosition = transform.position + m_param.offset;
        var rayDirection = CalculateRayDirection(tacticle);
        var targetPosition = startPosition + rayDirection;

        //���C���q�b�g������A����������Ԃ��B
        int obstacleLayer = LayerMask.GetMask(maru.UtilityObstacle.DEFAULT_RAY_OBSTACLE_LAYER_STRINGS);
        var toVec = targetPosition - startPosition;

        //Instantiate(m_testerPrefab, (startPosition + rayDirection), Quaternion.identity);
        
        const float SphereRange = 0.5f;
        var colliders = Physics.OverlapSphere(transform.position, SphereRange, obstacleLayer);  //�I�u�W�F�N�g�ɐڐG���ɓ�����o�O�����p
        RaycastHit hitData;
        var hit = Physics.Raycast(startPosition, rayDirection, out hitData, m_param.avoidRange, obstacleLayer);

        if (hit) {
            Debug.Log("����Hit");
            result += -rayDirection;
        }

        return result.normalized * m_param.avoidPower;
    }

    private Vector3 CalculateRayDirection(WallAvoidTacticle tacticle)
    {
        var forward = transform.forward;
        var direction = Quaternion.AngleAxis(tacticle.GetRad() * Mathf.Rad2Deg, Vector3.up) * forward;

        return direction.normalized * tacticle.GetRange();
    }

    public Vector3 TakeAvoidVector()
    {
        var result = m_avoidVec;
        m_avoidVec = Vector3.zero;
        return result;
    }

}
