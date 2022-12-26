using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GraphEdge : I_GraphEdge
{
    private I_GraphNode m_fromNode;   //手前のノード
    private I_GraphNode m_toNode;     //先のノード

    public GraphEdge(GraphNode fromNode, GraphNode toNode)
    {
        m_fromNode = fromNode;
        m_toNode = toNode;
    }

    public void SetFromNode(I_GraphNode node) { m_fromNode = node; }

    public I_GraphNode GetFromNode() { return m_fromNode; }

    public int GetFromIndex() { return m_fromNode.GetIndex(); }

    public void SetToNode(I_GraphNode node) { m_toNode = node; }

    public I_GraphNode GetToNode() { return m_toNode; }

    public int GetToIndex() { return m_toNode.GetIndex(); }
}
