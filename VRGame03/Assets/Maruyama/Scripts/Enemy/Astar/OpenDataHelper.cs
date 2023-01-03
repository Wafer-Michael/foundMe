using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using System.Linq;

using GraphType = SparseGraph<AstarNode, AstarEdge>;

public class OpenData
{
    public AstarNode parent;    //自分の前のノード
    public AstarNode node;      //自分自身のノードポインタ
    public float range;         //実距離
    public float heuristic;     //ヒュースリック距離
    public bool isOpen;

    public OpenData(AstarNode parent, AstarNode selfNode, float range, float heuristic)
    {
        this.parent = parent;
        this.node = selfNode;
        this.range = range;
        this.heuristic = heuristic;
        this.isOpen = true;
    }

    public float GetSumRange() { return range + heuristic; }

    //public static bool operator ==(OpenData left, OpenData right) {
    //    //親と自分自身が同じなら

    //    if(left.parent == null && right.parent == null) {
    //        return left.node == right.node;
    //    }

    //    if(left.parent == null || right.parent == null) {
    //        return false;
    //    }

    //    return ( left.parent == right.parent &&
    //             left.node == right.node);
    //}

    //public static bool operator !=(OpenData left, OpenData right) { 
    //    return !(left == right); 
    //}

    public bool IsEqual(OpenData other)
    {
        return (this.parent == other.parent &&
                this.node == other.node);
    }

    public void move(OpenData other)
    {
        this.parent = other.parent;
        this.node = other.node;
        this.range = other.range;
        this.heuristic = other.heuristic;
        this.isOpen = other.isOpen;
    }
}

public class OpenDataHelper
{
    private Stack<AstarNode> m_route;   //生成したルート

    private AstarNode m_otherAreaNode;  //別エリアノード

    public OpenDataHelper()
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

    private bool CreateOpenDatas(
        ref List<OpenData> openDatas,
        ref List<OpenData> closeDatas,
        OpenData openData,
        GraphType graph,
        AstarNode startNode,
        AstarNode targetNode,
        int targetAreaIndex = -1
    ) {
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
            if (node == null) {
                continue;
            }

            var toNodeVec = node.GetPosition() - baseNode.GetPosition();
            float range = toNodeVec.magnitude;
            range += openData.range;
            float heuristicRange = (targetNode.GetPosition() - node.GetPosition()).magnitude;

            int newIndex = node.GetIndex();
            var newData = new OpenData(baseNode, node, range, heuristicRange);

            bool isAddData = AddOpenData(openDatas, closeDatas, newData);

            //目標ノードなら終わらせる
            if(node == targetNode) {
                return true;
            }

            //目標エリアが他エリアで、かつ、目標エリアのノードならotherAreaOpenDatasに登録する
            if (IsOtherAreaTarget(startNode, targetAreaIndex) && node.GetParent().GetIndex() == targetAreaIndex)
            {
                otherAreaOpenDatas.Add(newData);
            }
        }

        //別エリアのデータが存在するなら、別エリアとの境目で探索終了
        if(otherAreaOpenDatas.Count != 0) {
            var sotrDatas = otherAreaOpenDatas.OrderBy(value => value.GetSumRange());
            m_otherAreaNode = sotrDatas.ToArray()[0].node;
            //return true;  //今回エリアごとの変更がうまくいくか判断できないから保留
        }

        return false;
    }

    private bool CreateRoute(List<OpenData> openDatas, AstarNode targetNode)
    {
        int index = 0;
        const int TempMaxIndex = 100000;

        var tempData = FindSomeNodeOpenData(openDatas, targetNode);

        while(index <= TempMaxIndex)
        {
            if (tempData.parent == null) {  //親ノードが存在しないなら、処理を終了
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
            if(data.node == node) {
                return data;
            }
        }

        return null;
    }

    private bool IsRegisterData(List<OpenData> openDatas, OpenData openData)
    {
        foreach(var data in openDatas)
        {
            if(data.IsEqual(openData)) {
                return true;
            }
        }

        return false;
    }

    private bool AddOpenData(
        List<OpenData> openDatas, 
        List<OpenData> closeDatas, 
        OpenData newData
    ) {
        var someOpenData = FindSomeNodeOpenData(openDatas, newData.node);
        var someCloseData = FindSomeNodeOpenData(closeDatas, newData.node);
        
        //オープンリストに登録されていない、かつ、クローズリストに登録されていない
        if(someOpenData == null && someCloseData == null)
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
        if(someCloseData != null && !IsSmall_LeftOpenData(someCloseData, newData))
        {
            openDatas.Add(newData);
            closeDatas.Remove(someCloseData);
            return true;
        }

        //どの条件にも当てはまらないなら、追加をしない
        return false;
    }

    public bool StartSearch(
        AstarNode startNode,
        AstarNode targetNode,
        GraphType graph,
        int targetAreaIndex = -1
    ) {
        m_route.Clear();
        m_otherAreaNode = null;

        //オープンデータリストとクローズデータリストを作成
        var openDatas = new List<OpenData>();
        var closeDatas = new List<OpenData>();

        //初期データの生成
        openDatas.Add(new OpenData(null, startNode, 0.0f, CalculateHeuristicRange(startNode, targetNode)));

        //オープンデータが存在する限りループする。
        while(openDatas.Count != 0)
        {
            //オープンデータ生成用の基準ノードの生成
            var baseOpenData = FindBaseOpenData(openDatas);

            //オープンデータの生成。ターゲットノードにたどり着いたらtrueを返す。
            if(CreateOpenDatas(ref openDatas, ref closeDatas, baseOpenData, graph, startNode, targetNode)) {
                break;
            }
        }

        bool isAstarSuccess = (openDatas.Count != 0);  //オープンデータが存在するなら、検索失敗

        if (!isAstarSuccess) {  //失敗したら次の処理を飛ばす。
            return isAstarSuccess;
        }

        //クローズデータをオープンデータに含める。
        foreach(var closeData in closeDatas)
        {
            openDatas.Add(closeData);
        }

        //var lastTargetNode = m_otherAreaNode != null ? m_otherAreaNode : targetNode;
        var lastTargetNode = targetNode;
        var isCreateRoute = CreateRoute(openDatas, lastTargetNode);
        if (!isCreateRoute) {
            Debug.Log("ルート生成に失敗しました。");
        }

        return isAstarSuccess;
    }

    public Stack<AstarNode> GetRoute() { return m_route; }

    private bool IsSmall_LeftOpenData(OpenData left, OpenData right)
    {
        return left.GetSumRange() < right.GetSumRange();
    }

    private float CalculateHeuristicRange(AstarNode targetNode, AstarNode selfMode)
    {
        return (targetNode.GetPosition() - selfMode.GetPosition()).magnitude;
    }
}
