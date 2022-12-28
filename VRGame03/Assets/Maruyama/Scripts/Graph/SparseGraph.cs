using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//--------------------------------------------------------------------------------------
///	(2022/12/26)�ŐV�O���t
//--------------------------------------------------------------------------------------
public class SparseGraph<NodeType, EdgeType>
    where NodeType : GraphNode
    where EdgeType : GraphEdge
{
    private bool m_isActive;

    private List<NodeType> m_nodes = new List<NodeType>();  //�m�[�h�S��

    private Dictionary<int, List<EdgeType>> m_edgesMap = new Dictionary<int, List<EdgeType>>(); //�G�b�W�}�b�v

    public SparseGraph() 
    {
        m_isActive = true;
    }

    public NodeType AddNode(NodeType node)
    {
        m_nodes.Add(node);
        return node;
    }

    public NodeType GetNode(int index) {
        foreach(var node in m_nodes)
        {
            if(node.GetIndex() == index) {
                return node;
            }
        }

        return null;

        //if (index >= m_nodes.Count) {   //�m�[�h�����傫���C���f�b�N�X���w�肵����null��Ԃ��B
        //    return null;
        //}

        //return m_nodes[index];
    }

    public List<NodeType> GetNodes() { 
        return m_nodes; 
    }

    public int GetNumNode() {
        return m_nodes.Count;
    }

    /// <summary>
    /// �����C���f�b�N�X�̃m�[�h�����݂��邩�ǂ���
    /// </summary>
    /// <param name="index">�C���f�b�N�X</param>
    /// <returns>���݂���Ȃ�true</returns>
    public bool IsSomeIndexNode(int index) {
        return (index >= m_nodes.Count);
    }

    public EdgeType AddEdge(EdgeType edge)
    {
        int fromIndex = edge.GetFromIndex();
        if (!m_edgesMap.ContainsKey(fromIndex)) {           //�܂����݂��Ȃ��C���f�b�N�X�Ȃ�
            m_edgesMap[fromIndex] = new List<EdgeType>();   //new List�̐���
        }

        m_edgesMap[fromIndex].Add(edge);
        return edge;
    }

    public EdgeType GetEdge(int fromIndex, int toIndex) {
        if (!m_edgesMap.ContainsKey(fromIndex)) {   //�L�[�����݂��Ȃ��Ȃ�
            return null;
        }

        //toIndex�ƈ�v����Ȃ炻�̃G�b�W��Ԃ��B
        foreach(var edge in m_edgesMap[fromIndex])
        {
            if(edge.GetToIndex() == toIndex)
            {
                return edge;
            }
        }

        return null;
    }

    public List<EdgeType> GetEdges(int index)
    {
        if (!m_edgesMap.ContainsKey(index)) {
            return null;
        }

        return m_edgesMap[index];
    }

    public Dictionary<int, List<EdgeType>> GetEdgesMap() { 
        return m_edgesMap;
    }

    public int GetNumEdge(int index) {
        if (!m_edgesMap.ContainsKey(index)) {   //�L�[�����݂��Ȃ��Ȃ�
            return 0;
        }

        return m_edgesMap[index].Count;
    }

    /// <summary>
    /// �����G�b�W�����݂��邩�ǂ���
    /// </summary>
    /// <param name="fromIndex"></param>
    /// <param name="toIndex"></param>
    /// <returns>�����G�b�W�����݂���Ȃ�true</returns>
    public bool IsSomeIndexEdge(int fromIndex, int toIndex) {
        if (!m_edgesMap.ContainsKey(fromIndex)) {   //�L�[�����݂��Ȃ��Ȃ�
            return false;
        }

        foreach(var edge in m_edgesMap[fromIndex]) {
            if(edge.GetToIndex() == toIndex) {
                return true;
            }
        }

        return false;
    }

    /// <summary>
    /// ���ɒǉ�����m�[�h�̃C���f�b�N�X���擾
    /// </summary>
    /// <returns>���ɒǉ�����m�[�h�̃C���f�b�N�X</returns>
    public int GetNextNodeIndex() { return m_nodes.Count; }

    public bool IsActive() { return m_isActive; }

    public bool IsEmpty() { return m_nodes.Count == 0; }
}
