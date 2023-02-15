using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using GraphType = SparseGraph<AstarNode, AstarEdge>;

[RequireComponent(typeof(VelocityManager))]
[RequireComponent(typeof(TargetManager))]
public class AstarSeek : MonoBehaviour
{
    public static readonly Parametor DEFAULT_PARAMETOR = new Parametor(1.0f, 5.0f);

    [System.Serializable]
    public struct Parametor
    {
        public float nearRange; //近くと判断する距離
        public float maxSpeed;  //最大移動スピード   

        public Parametor(float nearRange, float maxSpeed)
        {
            this.nearRange = nearRange;
            this.maxSpeed = maxSpeed;
        }
    }

    private Stack<AstarNode> m_route;   //現在のルート
    private AstarNode m_currentNode;    //現在ターゲットにしているノード

    private TargetManager m_targetManager;
    private VelocityManager m_velocityManager;
    private RotationController m_rotationController;

    [SerializeField]
    private Parametor m_param = DEFAULT_PARAMETOR;

    private void Awake()
    {
        m_targetManager = GetComponent<TargetManager>();
        m_velocityManager = GetComponent<VelocityManager>();
        m_rotationController = GetComponent<RotationController>();

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

        if(IsNextRoute()) { //次のルートに遷移可能なら
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
        m_velocityManager.AddForce(force);
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
        //次のルートが存在しないなら
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
            Debug.Log("AstarNodeが存在しません");
            return;
        }

        //状態のリセット
        m_currentNode = null;
        m_route.Clear();

        var helper = new OpenDataHelper();

        helper.StartSearch(startNode, targetNode, graph, targetAreaIndex);

        var route = helper.GetRoute();
        SetRoute(helper.GetRoute());

        NextRoute();    //ルートの設定
    }

    //--------------------------------------------------------------------------------------
    /// アクセッサ
    //--------------------------------------------------------------------------------------

    private void SetRoute(Stack<AstarNode> route) { m_route = route; }

    public void SetMaxSpeed(float speed) { m_param.maxSpeed = speed; }

    public float GetMaxSpeed() { return m_param.maxSpeed; }

    public bool IsEnd() { return m_currentNode == null; }

    public void SetParametor(Parametor parametor) { m_param = parametor; }

    public Parametor GetParametor() { return m_param; }
}
