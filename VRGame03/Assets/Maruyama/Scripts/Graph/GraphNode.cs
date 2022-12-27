using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GraphNode : I_GraphNode
{
    private int m_index;        //�C���f�b�N�X

    private bool m_isActive;    //�m�[�h�̃A�N�e�B�u���

    public GraphNode(int index)
    {
        m_index = index;
        m_isActive = true;
    }

    public void SetIndex(int index) { m_index = index; }

    public int GetIndex() { return m_index; }

    public bool IsActive() { return m_isActive; }
}
