using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using GraphType = SparseGraph<AstarNode, AstarEdge>;

[RequireComponent(typeof(VelocityManager))]
[RequireComponent(typeof(TargetManager))]
public class AstarSeek : MonoBehaviour
{
    public static readonly Parametor DEFAULT_PARAMETOR = new Parametor(1.0f, 2.0f);

    [System.Serializable]
    public struct Parametor
    {
        public float nearRange; //�߂��Ɣ��f���鋗��
        public float maxSpeed;  //�ő�ړ��X�s�[�h   

        public Parametor(float nearRange, float maxSpeed)
        {
            this.nearRange = nearRange;
            this.maxSpeed = maxSpeed;
        }
    }

    private Stack<AstarNode> m_route;   //���݂̃��[�g
    private AstarNode m_currentNode;    //���݃^�[�Q�b�g�ɂ��Ă���m�[�h

    private VelocityManager m_velocityManager;
    private RotationController m_rotationController;
    private WallAvoid m_wallAvoid;

    [SerializeField]
    private Parametor m_param = DEFAULT_PARAMETOR;

    private void Awake()
    {
        m_velocityManager = GetComponent<VelocityManager>();
        m_rotationController = GetComponent<RotationController>();
        m_wallAvoid = GetComponent<WallAvoid>();

        m_route = new Stack<AstarNode>();
        m_currentNode = null;
    }

    public void Update()
    {
        if (IsEnd()) {
            return;
        }

        UpdateMove();
        UpdateRotation();

        if(IsNextRoute()) { //���̃��[�g�ɑJ�ډ\�Ȃ�
            NextRoute();
        }
    }

    private void UpdateMove()
    {
        if (m_currentNode == null) {
            return;
        }

        var toVec = m_currentNode.GetPosition() - transform.position;

        var force = maru.CalculateVelocity.SeekVec(m_velocityManager.velocity, toVec.normalized, m_param.maxSpeed);

        var velocity = m_velocityManager.velocity;              //���x�̎擾
        var moveDirection = velocity + force;                   //�����ɐV�K�x�N�g����ǉ�
        moveDirection += m_wallAvoid.TakeAvoidVector();         //

        moveDirection /= m_velocityManager.GetMaxSpeed();       //0 �` 1�̊ԂɕύX�B

        var input = new Vector2(moveDirection.x, moveDirection.z);

        m_velocityManager.velocity = moveDirection * m_velocityManager.GetMaxSpeed();
    }

    private void UpdateRotation()
    {
        m_rotationController.SetDirection(m_velocityManager.velocity);
    }

    private bool IsNextRoute()
    {
        if(m_currentNode == null) {
            return false;
        }

        return ToCurrentNodeRange() < m_param.nearRange;
    }

    private void NextRoute()
    {
        //���̃��[�g�����݂��Ȃ��Ȃ�
        if(m_route.Count == 0) {
            m_currentNode = null;
            return;
        }

        m_currentNode = m_route.Pop();
    }

    private float ToCurrentNodeRange()
    {
        return (m_currentNode.GetPosition() - transform.position).magnitude;
    }

    public void StartAstar(
        AstarNode startNode,
        AstarNode targetNode,
        GraphType graph,
        int targetAreaIndex = -1
    ) {
        if(startNode == null || targetNode == null) {
            Debug.Log("AstarNode�����݂��܂���");
            return;
        }

        //��Ԃ̃��Z�b�g
        m_currentNode = null;
        m_route.Clear();

        var helper = new OpenDataHelper();

        helper.StartSearch(startNode, targetNode, graph, targetAreaIndex);

        var route = helper.GetRoute();
        SetRoute(helper.GetRoute());

        NextRoute();    //���[�g�̐ݒ�
    }

    public void StartAstar(
        AstarNode startNode,
        Vector3 targetPosition,
        GraphType graph,
        float nearTargetRange,
        int targetAreaIndex = -1
    ) {
        if (startNode == null)
        {
            Debug.Log("AstarNode�����݂��܂���");
            return;
        }

        //��Ԃ̃��Z�b�g
        m_currentNode = null;
        m_route.Clear();

        var helper = new OpenDataHelper_Ex();

        helper.StartSearch(startNode, targetPosition, graph, nearTargetRange, targetAreaIndex);

        var route = helper.GetRoute();
        SetRoute(helper.GetRoute());

        NextRoute();    //���[�g�̐ݒ�
    }

    //--------------------------------------------------------------------------------------
    /// �A�N�Z�b�T
    //--------------------------------------------------------------------------------------

    private void SetRoute(Stack<AstarNode> route) { m_route = route; }

    public void SetMaxSpeed(float speed) { m_param.maxSpeed = speed; }

    public float GetMaxSpeed() { return m_param.maxSpeed; }

    public bool IsEnd() { return m_currentNode == null; }

    public void SetParametor(Parametor parametor) { m_param = parametor; }

    public Parametor GetParametor() { return m_param; }
}
