using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using GraphType = SparseGraph<AstarNode, AstarEdge>;

public class WayPointsMap
{
    private GraphType m_graph;  //�O���t�f�[�^

    public WayPointsMap() {
        m_graph = new GraphType();
    }

    /// <summary>
    /// �E�F�C�|�C���g�}�b�v�̐���
    /// </summary>
    public void CreateWayPointsMap(Factory.WayPointsMap_FloodFill.Parametor factoryParametor)
    {
        var factory = new Factory.WayPointsMap_FloodFill();

        factory.AddWayPointMap(m_graph, factoryParametor);
    }

    //--------------------------------------------------------------------------------------
    /// �A�N�Z�b�T
    //--------------------------------------------------------------------------------------

    public void SetGraph(GraphType graph) { m_graph = graph; }

    public GraphType GetGraph() { return m_graph; }

    public float GetIntervalRange() { 
        if(m_graph.GetNodes().Count <= 1) { //�m�[�h��������Ȃ��Ȃ�A�m�[�h�Ԃ̒������֌W�Ȃ�
            return 0.0f;
        }

        var oneNode = m_graph.GetNode(0);
        var twoNode = m_graph.GetNode(1);

        return (oneNode.GetPosition() - twoNode.GetPosition()).magnitude;
    }
}
