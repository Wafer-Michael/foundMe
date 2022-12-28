using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using GraphType = SparseGraph<AstarNode, AstarEdge>;

public class DebugGraphDraw
{
    private MonoBehaviour m_owner;            //このクラスの所有者
    public MonoBehaviour Owner => m_owner;

    readonly private GraphType m_graph;    //グラフ

    private List<GameObject> m_nodes = new List<GameObject>();  //デバッグ用のノード
    private List<GameObject> m_edges = new List<GameObject>();  //デバッグ用のエッジ

    public DebugGraphDraw(MonoBehaviour owner, GraphType graph)
    {
        m_owner = owner;
        m_graph = graph;
    }

    public void CreateDebugNodes(GameObject prefab)
    {
        foreach (var node in m_graph.GetNodes())
        {
            var drawObject = Object.Instantiate(prefab, node.GetPosition(), Quaternion.identity);
            m_nodes.Add(drawObject);
        }
    }

    public void CreateDebugEdges(GameObject prefab)
    {
        foreach(var pair in m_graph.GetEdgesMap())
        {
            foreach(var edge in pair.Value)
            {
                var fromNode = m_graph.GetNode(edge.GetFromIndex());
                var toNode = m_graph.GetNode(edge.GetToIndex());

                var position = (fromNode.GetPosition() + toNode.GetPosition()) / 2.0f;

                var drawObject = Object.Instantiate(prefab, position, Quaternion.identity);
                m_edges.Add(drawObject);
            }
        }
    }

    //--------------------------------------------------------------------------------------
    ///	アクセッサ
    //--------------------------------------------------------------------------------------

}
