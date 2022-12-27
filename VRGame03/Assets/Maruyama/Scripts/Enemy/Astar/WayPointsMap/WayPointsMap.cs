using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using GraphType = SparseGraph<AstarNode, AstarEdge>;

public class WayPointsMap
{
    private GraphType m_graph;  //グラフデータ

    public WayPointsMap()
    {
        m_graph = new GraphType();
    }

    /// <summary>
    /// ウェイポイントマップの生成
    /// </summary>
    public void CreateWayPointsMap(Factory.WayPointsMap_FloodFill.Parametor factoryParametor)
    {
        var factory = new Factory.WayPointsMap_FloodFill();

        factory.AddWayPointMap(m_graph, factoryParametor);
    }

    //--------------------------------------------------------------------------------------
    /// アクセッサ
    //--------------------------------------------------------------------------------------

    public void SetGraph(GraphType graph) { m_graph = graph; }

    public GraphType GetGraph() { return m_graph; }
}
