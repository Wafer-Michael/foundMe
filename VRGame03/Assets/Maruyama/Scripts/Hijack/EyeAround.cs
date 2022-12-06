using UnityEngine;


//--------------------------------------------------------------------------------------
/// ���E�Ŏ�����m�F����R���|�[�l���g
//--------------------------------------------------------------------------------------
[RequireComponent(typeof(RotationController))]
public class EyeAround : MonoBehaviour
{
    [System.Serializable]
    struct Parametor {
        public float speed;         //�X�s�[�h
        public float degree;        //�p�x
        public Vector3 direction;   //��]����

        public float Radian => Mathf.Deg2Rad * degree;  //���W�A���p�ɕϊ�
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
        //��]���I��������
        if (!m_rotationController.IsRotation)
        {
            StartRotation();
        }
    }

    public void StartRotation()
    {
        var direction = CalculateDirection(m_rotationDirection);
        m_rotationDirection *= -1;  //���͔��]������B

        m_rotationController.SetDirection(direction);
    }

    public Vector3 CalculateDirection(int direction)
    {
        return Quaternion.Euler(m_param.direction * m_param.degree * direction) * m_startForward;  //��]������B
    }

}
