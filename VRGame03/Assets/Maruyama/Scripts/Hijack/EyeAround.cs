using UnityEngine;


//--------------------------------------------------------------------------------------
/// 視界で周りを確認するコンポーネント
//--------------------------------------------------------------------------------------
[RequireComponent(typeof(RotationController))]
public class EyeAround : MonoBehaviour
{
    [System.Serializable]
    struct Parametor {
        public float speed;         //スピード
        public float degree;        //角度
        public Vector3 direction;   //回転方向

        public float Radian => Mathf.Deg2Rad * degree;  //ラジアン角に変換
    }

    [SerializeField]
    Parametor m_param;

    Vector3 m_startForward;
    int m_rotationDirection = 1;

    RotationController m_rotationController;

    private void Awake()
    {
        m_rotationController = GetComponent<RotationController>();
    }

    private void Start()
    {
        m_startForward = transform.forward;

        m_rotationController.SetSpeed(m_param.speed);

        StartRotation();
    }

    private void Update()
    {
        //回転が終了したら
        if (!m_rotationController.IsRotation)
        {
            StartRotation();
        }
    }

    public void StartRotation()
    {
        var direction = CalculateDirection(m_rotationDirection);
        m_rotationDirection *= -1;  //次は反転させる。

        m_rotationController.SetDirection(direction);
    }

    public Vector3 CalculateDirection(int direction)
    {
        return Quaternion.Euler(m_param.direction * m_param.degree * direction) * m_startForward;  //回転させる。
    }

}
