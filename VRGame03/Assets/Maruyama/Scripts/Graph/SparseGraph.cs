using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//--------------------------------------------------------------------------------------
///	(2022/12/26)最新グラフ
//--------------------------------------------------------------------------------------
public class SparseGraph<NodeType, EdgeType>
    where NodeType : GraphNode
    where EdgeType : GraphEdge
{
    private bool m_isActive;

    private List<NodeType> m_nodes = new List<NodeType>();  //ノード全て

    private Dictionary<int, List<EdgeType>> m_edgesMap = new Dictionary<int, List<EdgeType>>(); //エッジマップ

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
        if (index >= m_nodes.Count) {   //ノード数より大きいインデックスを指定したらnullを返す。
            return null;
        }

        return m_nodes[index];
    }

    public List<NodeType> GetNodes() { 
        return m_nodes; 
    }

    public int GetNumNode() {
        return m_nodes.Count;
    }

    /// <summary>
    /// 同じインデックスのノードが存在するかどうか
    /// </summary>
    /// <param name="index">インデックス</param>
    /// <returns>存在するならtrue</returns>
    public bool IsSomeIndexNode(int index) {
        return (index >= m_nodes.Count);
    }

    public EdgeType AddEdge(EdgeType edge)
    {
        int fromIndex = edge.GetFromIndex();
        if (!m_edgesMap.ContainsKey(fromIndex)) {           //まだ存在しないインデックスなら
            m_edgesMap[fromIndex] = new List<EdgeType>();   //new Listの生成
        }

        m_edgesMap[fromIndex].Add(edge);
        return edge;
    }

    public EdgeType GetEdge(int fromIndex, int toIndex) {
        if (!m_edgesMap.ContainsKey(fromIndex)) {   //キーが存在しないなら
            return null;
        }

        //toIndexと一致するならそのエッジを返す。
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
        if (!m_edgesMap.ContainsKey(index)) {   //キーが存在しないなら
            return 0;
        }

        return m_edgesMap[index].Count;
    }

    /// <summary>
    /// 同じエッジが存在するかどうか
    /// </summary>
    /// <param name="fromIndex"></param>
    /// <param name="toIndex"></param>
    /// <returns>同じエッジが存在するならtrue</returns>
    public bool IsSomeIndexEdge(int fromIndex, int toIndex) {
        if (!m_edgesMap.ContainsKey(fromIndex)) {   //キーが存在しないなら
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
    /// 次に追加するノードのインデックスを取得
    /// </summary>
    /// <returns>次に追加するノードのインデックス</returns>
    public int GetNextNodeIndex() { return m_nodes.Count; }

    public bool IsActive() { return m_isActive; }

    public bool IsEmpty() { return m_nodes.Count == 0; }
}
