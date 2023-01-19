using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using System.Linq;

using GraphType = SparseGraph<AstarNode, AstarEdge>;

public class OpenDataHelper_Ex : MonoBehaviour
{
    private Stack<AstarNode> m_route;   //生成したルート

    private AstarNode m_otherAreaNode;  //別エリアノード

    public OpenDataHelper_Ex()
    {
        m_route = new Stack<AstarNode>();
        m_otherAreaNode = null;
    }

    /// <summary>
    /// オープンデータを生成するための基準となるオープンデータを検索(一番期待値の高いノードを取得する。)
    /// </summary>
    /// <returns>オープンデータを生成するための開始ノード</returns>
    private OpenData FindBaseOpenData(List<OpenData> openDatas)
    {
        var sortDatas = openDatas.OrderBy(value => { return value.GetSumRange(); });

        return sortDatas.ToArray()[0];
    }

    private bool IsOtherAreaTarget(AstarNode startNode, int targetAreaIndex)
    {
        //目標エリアが0以上かつ、目標ノードが目標エリアと違うとき
        return (targetAreaIndex >= 0 && targetAreaIndex != startNode.GetParent().GetIndex());
    }

    private AstarNode CreateOpenDatas(
        ref List<OpenData> openDatas,
        ref List<OpenData> closeDatas,
        OpenData openData,
        GraphType graph,
        AstarNode startNode,
        Vector3 targetPosition,
        float nearTargetRange,
        int targetAreaIndex = -1
    )
    {
        var baseNode = openData.node;
        var edges = graph.GetEdges(baseNode.GetIndex());
        int baseIndex = baseNode.GetIndex();

        openDatas.Remove(openData); //使用するオープンデータをリストから削除
        closeDatas.Add(openData);   //使用するオープンデータをクローズリストに追加

        List<OpenData> otherAreaOpenDatas = new List<OpenData>();

        foreach (var edge in edges)
        {
            //var node = graph.GetNode(edge.GetToIndex());    //ノードの取得
            var node = edge.GetToNode() as AstarNode;
            if (node == null)
            {
                continue;
            }

            var toNodeVec = node.GetPosition() - baseNode.GetPosition();
            float range = toNodeVec.magnitude;
            range += openData.range;
            float heuristicRange = (targetPosition - node.GetPosition()).magnitude;

            int newIndex = node.GetIndex();
            var newData = new OpenData(baseNode, node, range, heuristicRange);

            bool isAddData = AddOpenData(openDatas, closeDatas, newData);

            //目標ノードなら終わらせる
            if (IsNearTargetNode(node, targetPosition, nearTargetRange)) {
                return node;
            }

            //目標エリアが他エリアで、かつ、目標エリアのノードならotherAreaOpenDatasに登録する
            if (IsOtherAreaTarget(startNode, targetAreaIndex) && node.GetParent().GetIndex() == targetAreaIndex) {
                otherAreaOpenDatas.Add(newData);
            }
        }

        //別エリアのデータが存在するなら、別エリアとの境目で探索終了
        if (otherAreaOpenDatas.Count != 0)
        {
            var sotrDatas = otherAreaOpenDatas.OrderBy(value => value.GetSumRange());
            m_otherAreaNode = sotrDatas.ToArray()[0].node;
            //return true;  //今回エリアごとの変更がうまくいくか判断できないから保留
        }

        return null;
    }

    private bool CreateRoute(List<OpenData> openDatas, AstarNode targetNode)
    {
        int index = 0;
        const int TempMaxIndex = 100000;

        var tempData = FindSomeNodeOpenData(openDatas, targetNode);

        while (index <= TempMaxIndex)
        {
            if (tempData.parent == null)
            {  //親ノードが存在しないなら、処理を終了
                break;
            }

            m_route.Push(tempData.node);
            tempData = FindSomeNodeOpenData(openDatas, tempData.parent);
            index++;
        }

        return index <= TempMaxIndex;
    }

    private OpenData FindSomeNodeOpenData(List<OpenData> openDatas, AstarNode node)
    {
        foreach (var data in openDatas)
        {
            if (data.node == node)
            {
                return data;
            }
        }

        return null;
    }

    private bool IsRegisterData(List<OpenData> openDatas, OpenData openData)
    {
        foreach (var data in openDatas)
        {
            if (data.IsEqual(openData))
            {
                return true;
            }
        }

        return false;
    }

    private bool AddOpenData(
        List<OpenData> openDatas,
        List<OpenData> closeDatas,
        OpenData newData
    )
    {
        var someOpenData = FindSomeNodeOpenData(openDatas, newData.node);
        var someCloseData = FindSomeNodeOpenData(closeDatas, newData.node);

        //オープンリストに登録されていない、かつ、クローズリストに登録されていない
        if (someOpenData == null && someCloseData == null)
        {
            openDatas.Add(newData);
            return true;
        }

        //オープンリストに登録されていて、新規データの方が小さい合計値
        if (someOpenData != null && IsSmall_LeftOpenData(someOpenData, newData))
        {
            someOpenData.move(newData);
            return true;
        }

        //クローズリストに登録されていて、新規データの方が小さい合計値
        if (someCloseData != null && !IsSmall_LeftOpenData(someCloseData, newData))
        {
            openDatas.Add(newData);
            closeDatas.Remove(someCloseData);
            return true;
        }

        //どの条件にも当てはまらないなら、追加をしない
        return false;
    }

    /// <summary>
    /// Astarの処理を開始する。
    /// </summary>
    /// <param name="startNode">開始ノード</param>
    /// <param name="targetPosition">目標位置</param>
    /// <param name="graph">グラフデータ</param>
    /// <param name="nearTargetRange">ターゲットとの近くと判断する距離</param>
    /// <param name="targetAreaIndex">ターゲットのエリアインデックス</param>
    /// <returns>Astarの処理が成功したかどうかを戻り値とする。</returns>
    public bool StartSearch(
        AstarNode startNode,
        Vector3 targetPosition,
        GraphType graph,
        float nearTargetRange,
        int targetAreaIndex = -1
    ) {
        m_route.Clear();
        m_otherAreaNode = null;

        //オープンデータリストとクローズデータリストを作成
        var openDatas = new List<OpenData>();
        var closeDatas = new List<OpenData>();

        //初期データの生成
        openDatas.Add(new OpenData(null, startNode, 0.0f, CalculateHeuristicRange(startNode, targetPosition)));
        AstarNode targetNode = null;

        //オープンデータが存在する限りループする。
        while (openDatas.Count != 0)
        {
            //オープンデータ生成用の基準ノードの生成
            var baseOpenData = FindBaseOpenData(openDatas);

            //オープンデータの生成。ターゲットノードにたどり着いたらtrueを返す。
            var endNode = CreateOpenDatas(ref openDatas, ref closeDatas, baseOpenData, graph, startNode, targetPosition, nearTargetRange);
            if (endNode != null)   //終了ノードが帰ってきたら
            {
                targetNode = endNode;
                break;
            }
        }

        bool isAstarSuccess = (openDatas.Count != 0);  //オープンデータが存在するなら、検索失敗

        if (!isAstarSuccess)
        {  //失敗したら次の処理を飛ばす。
            return isAstarSuccess;
        }

        //クローズデータをオープンデータに含める。
        foreach (var closeData in closeDatas)
        {
            openDatas.Add(closeData);
        }

        //var lastTargetNode = m_otherAreaNode != null ? m_otherAreaNode : targetNode;
        var lastTargetNode = targetNode;
        var isCreateRoute = CreateRoute(openDatas, lastTargetNode);
        if (!isCreateRoute)
        {
            Debug.Log("ルート生成に失敗しました。");
        }

        return isAstarSuccess;
    }

    public Stack<AstarNode> GetRoute() { return m_route; }

    private bool IsSmall_LeftOpenData(OpenData left, OpenData right)
    {
        return left.GetSumRange() < right.GetSumRange();
    }

    private float CalculateHeuristicRange(AstarNode selfNode, AstarNode targetNode)
    {
        return (targetNode.GetPosition() - selfNode.GetPosition()).magnitude;
    }

    private float CalculateHeuristicRange(AstarNode selfNode, Vector3 targetPosition)
    {
        return (targetPosition - selfNode.GetPosition()).magnitude;
    }

    /// <summary>
    /// ターゲットノードの近くまで来たかどうか
    /// </summary>
    /// <param name="currentNode"></param>
    /// <param name="targetPosition"></param>
    /// <param name="nearTargetRange"></param>
    /// <returns></returns>
    private bool IsNearTargetNode(AstarNode currentNode, Vector3 targetPosition, float nearTargetRange)
    {
        //四角範囲内でないなら、まだ遠いから処理を飛ばす。
        var rect = new maru.Rect(targetPosition, nearTargetRange, nearTargetRange);
        if (!rect.IsInRect(currentNode.GetPosition())) {
            return false;
        }

        //障害物の外かどうかを判断する。
        var direction = targetPosition - currentNode.GetPosition();
        var layerMask = LayerMask.GetMask(maru.UtilityObstacle.DEFAULT_RAY_OBSTACLE_LAYER_STRINGS);
        if(Physics.Raycast(currentNode.GetPosition(), direction, nearTargetRange, layerMask))  
        {
            //当たったならターゲットノードでない
            return false;
        }

        return true;
    }
}
