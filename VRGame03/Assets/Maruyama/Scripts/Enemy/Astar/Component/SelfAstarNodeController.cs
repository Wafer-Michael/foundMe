using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using System.Linq;
using System.Threading;
using System.Threading.Tasks;

public class SelfAstarNodeController : MonoBehaviour
{
    private AstarNode m_node = null;

    private bool m_isInitialize = false;    //初期化中かどうか

    [SerializeField]
    private bool m_isMover = true;  //移動するため、常に監視が必要かどうか

    private void Start()
    {
        InitializeNode();
    }

    private void Update()
    {
        //移動する者のみ更新処理をかける。
        if (IsMover()) {
            UpdateProcess();
        }

        if (HasNode()) {
            //Debug.Log("★" + GetNode().GetIndex().ToString());
        }
    }

    private void UpdateProcess()
    {
        if (IsInitialize()) {   //初期化中なら処理を飛ばす
            return;
        }

        //ノードがない、又は、現在のノードからかなり離れている場合
        if(!HasNode() || IsFarRange()) {
            InitializeNode();
            return;
        }

        //ノードの更新が必要なら
        if (IsUpdateNode()) {
            UpdateNode();
        }
    }

    /// <summary>
    /// ノードの初期化
    /// </summary>
    private void InitializeNode()
    {
        if (IsInitialize()) {   //初期化中なら呼ばない。
            return;
        }

        var position = transform.position;
        _ = Task.Run(() => InitializeProcess(position));    //呼ぶだけ読んで放置マルチスレッド(将来的にスレッドプールに変更)
    }

    private void InitializeProcess(Vector3 position)
    {
        m_isInitialize = true;
        var wayPointsMap = GetWayPointsMap();

        //検索
        m_node = maru.UtilityAstar.FindNearAstarNode(wayPointsMap.GetGraph(), position);

        m_isInitialize = false;

        Debug.Log("初期化終了");
    }

    private void UpdateNode()
    {
        //ノードを持ってて、動かないなら、
        if (HasNode() && !IsMover()) {
            return;
        }

        if (!HasNode()) {
            InitializeNode();
            return;
        }

        var edges = GetWayPointsMap().GetGraph().GetEdges(m_node.GetIndex());
        if(edges == null) {
            InitializeNode();
            return;
        }

        if(edges.Count == 0) {
            InitializeNode();
            return;
        }

        //一番近いノード順にソート
        var sotrEdges = edges.OrderBy(value => {
            var toNode = value.GetToNode() as AstarNode;
            return (toNode.GetPosition() - transform.position).magnitude;
        });

        var nextNode = sotrEdges.ToList()[0].GetToNode() as AstarNode;

        m_node = nextNode;
    }

    private bool IsUpdateNode()
    {
        if (!HasNode()) {   //ノードを持っていないなら、false
            return false;
        }

        var wayPointsMap = GetWayPointsMap();
        var intervalRange = wayPointsMap.GetIntervalRange();    //ノード間の距離を取得

        float range = (m_node.GetPosition() - transform.position).magnitude;

        return intervalRange < range;
    }

    private bool IsFarRange() {
        if (!HasNode()) {   //ノードを持っていないなら、false
            return false;
        }

        var wayPointsMap = GetWayPointsMap();
        var intervalRange = wayPointsMap.GetIntervalRange();    //ノード間の距離を取得
        const float FarCost = 1.0f; //仮のコスト(将来的に変更)
        float farRange = intervalRange + FarCost;

        float range = (m_node.GetPosition() - transform.position).magnitude;

        return farRange < range;   //ノードより距離が離れた場合
    }

    //--------------------------------------------------------------------------------------
    /// アクセッサ
    //--------------------------------------------------------------------------------------

    private void SetNode(AstarNode node) { m_node = node; }

    public AstarNode GetNode() { return m_node; }

    public bool HasNode() { return m_node != null; }

    private WayPointsMap GetWayPointsMap() { return AIDirector.Instance.GetWayPointsMap(); }

    public bool IsInitialize() { return m_isInitialize; }

    public bool IsMover() { return m_isMover; }

}
