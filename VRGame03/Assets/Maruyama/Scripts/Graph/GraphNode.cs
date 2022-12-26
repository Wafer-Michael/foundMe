using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GraphNode : I_GraphNode
{
    private int m_index;        //インデックス

    private bool m_isActive;    //ノードのアクティブ状態

    public GraphNode(int index)
    {
        m_index = index;
        m_isActive = true;
    }

    public void SetIndex(int index) { m_index = index; }

    public int GetIndex() { return m_index; }

    public bool IsActive() { return m_isActive; }
}
