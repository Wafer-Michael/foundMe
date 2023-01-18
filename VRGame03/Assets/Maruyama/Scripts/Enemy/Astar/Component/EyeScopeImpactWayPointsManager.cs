using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EyeScopeImpactWayPointsManager : MonoBehaviour
{
    private EyeSearchRange m_eyeRange;

    private SelfAstarNodeController m_selfAstarNodeController;

    private void Awake()
    {
        m_eyeRange = GetComponent<EyeSearchRange>();
        m_selfAstarNodeController = GetComponent<SelfAstarNodeController>();
    }

    private void Update()
    {
        var nodes = FindEyeScopeNodes_FloodFill();
        if(nodes == null) {
            return;
        }

        foreach(var node in nodes)
        {
            node.SetDangerValue(0);
        }
    }

    private Queue<AstarNode> FindEyeScopeNodes_FloodFill()
    {
        //現在位置のNodeが存在しないなら処理をしない
        if (!m_selfAstarNodeController.HasCurrentNode()) {
            return null;
        }

        AstarNode startNode = m_selfAstarNodeController.GetNode();
        var wayPointMap = AIDirector.Instance.GetWayPointsMap();

        var openNodes = new Queue<AstarNode>();
        var closeNodes = new Queue<AstarNode>();
        openNodes.Enqueue(startNode);

        while(openNodes.Count != 0)
        {
            var currentNode = openNodes.Dequeue();  //詮索するセルを取得
            closeNodes.Enqueue(currentNode);        //クローズリストに登録

            //八方向のセルを取得
            var edges = wayPointMap.GetGraph().GetEdges(currentNode.GetIndex());
            if(edges == null) {
                return null;
            }

            foreach (var edge in edges)
            {
                var node = edge.GetToNode() as AstarNode;
                if (node == null) {
                    continue;
                }

                //オープンデータに登録できるかどうか
                if (IsAddOpenNodes(node, openNodes, closeNodes)) {
                    openNodes.Enqueue(node);
                }
            }
        }

        return closeNodes;
    }

    private bool IsAddOpenNodes(AstarNode node, Queue<AstarNode> openNodes, Queue<AstarNode> closeNodes)
    {
        //すでにオープンデータに登録されている場合
        if (openNodes.Contains(node)) {
            return false;
        }

        //すでにクローズデータに登録されている場合
        if (closeNodes.Contains(node)) {
            return false;
        }

        //視界内に存在しない場合。
        if (!m_eyeRange.IsInEyeRange(node.GetPosition())) {
            return false;
        }

        return true;    //全ての条件をクリアしたため、視界内のセル
    }

}
