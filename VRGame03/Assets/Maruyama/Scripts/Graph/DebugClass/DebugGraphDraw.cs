using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using GraphType = SparseGraph<AstarNode, AstarEdge>;
using DrawType = DebugDrawComponent.DrawType;

public class DebugGraphDraw
{
    private MonoBehaviour m_owner;            //このクラスの所有者
    public MonoBehaviour Owner => m_owner;

    readonly private GraphType m_graph;    //グラフ

    private GameObject m_nodeParentObject;
    private GameObject m_edgeParentObject;

    private List<DebugDrawComponent> m_nodes = new List<DebugDrawComponent>();  //デバッグ用のノード
    private List<DebugDrawComponent> m_edges = new List<DebugDrawComponent>();  //デバッグ用のエッジ

    public DebugGraphDraw(MonoBehaviour owner, GraphType graph)
    {
        m_owner = owner;
        m_graph = graph;

        m_nodeParentObject = new GameObject("DebugNodes");
        m_edgeParentObject = new GameObject("DeubgEdges");
    }

    /// <summary>
    /// デバッグノードの生成
    /// </summary>
    /// <param name="prefab"></param>
    /// <param name="scale"></param>
    /// <param name="drawType"></param>
    /// <param name="color"></param>
    public void CreateDebugNodes(DebugDrawComponent prefab, Vector3? scale = null, DrawType drawType = DrawType.Cube, Color? color = null)
    {
        var passColor = new Color(0.0f, 0.0f, 0.0f, 0.3f);
        if(color != null) {
            passColor = color.Value;
        }

        CreateDebugNodes(prefab, scale, new DebugDrawComponent.Parametor(drawType, passColor, 0.5f));
    }

    /// <summary>
    /// デバッグノードの生成
    /// </summary>
    /// <param name="prefab"></param>
    /// <param name="scale"></param>
    /// <param name="drawParam"></param>
    public void CreateDebugNodes(
        DebugDrawComponent prefab, 
        Vector3? scale = null,
        DebugDrawComponent.Parametor? drawParam = null
    ) {
        foreach (var node in m_graph.GetNodes())
        {
            var debugDrawComponent = Object.Instantiate(prefab, node.GetPosition(), Quaternion.identity, m_nodeParentObject.transform);

            //スケール設定。
            if (scale != null) {
                debugDrawComponent.transform.localScale = (Vector3)scale;
            }

            //表示パラメータを設定する。
            if (drawParam != null) {
                debugDrawComponent.Param = drawParam.Value;
            }

            m_nodes.Add(debugDrawComponent);
        }
    }

    /// <summary>
    /// デバッグエッジの生成
    /// </summary>
    /// <param name="prefab"></param>
    /// <param name="color"></param>
    public void CreateDebugEdges(DebugDrawComponent prefab, Color? color = null)
    {
        foreach(var pair in m_graph.GetEdgesMap())
        {
            foreach(var edge in pair.Value)
            {
                var fromNode = m_graph.GetNode(edge.GetFromIndex());
                var toNode = m_graph.GetNode(edge.GetToIndex());

                var toNodeVec = toNode.GetPosition() - fromNode.GetPosition();
                var halfRange = toNodeVec.magnitude * 0.5f;  //エッジ間の距離の半分
                var position = (fromNode.GetPosition() + toNode.GetPosition()) / 2.0f;

                var drawObject = Object.Instantiate(prefab, position, Quaternion.identity, m_edgeParentObject.transform);
                drawObject.transform.localScale = new Vector3(0.25f, 0.0f, halfRange);  //スケール設定
                drawObject.transform.forward = toNodeVec.normalized;

                var debugDraw = drawObject.GetComponent<DebugDrawComponent>();
                if (debugDraw) {
                    if(color != null) {
                        debugDraw.GizmosColor = (Color)color;
                    }
                }

                m_edges.Add(drawObject);
            }
        }
    }

    //--------------------------------------------------------------------------------------
    ///	アクセッサ
    //--------------------------------------------------------------------------------------

    public List<DebugDrawComponent> GetNodes() { return m_nodes; }

    public List<DebugDrawComponent> GetEdges() { return m_edges; }

}
