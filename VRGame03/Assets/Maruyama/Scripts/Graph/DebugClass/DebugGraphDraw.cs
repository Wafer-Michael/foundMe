using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using GraphType = SparseGraph<AstarNode, AstarEdge>;
using DrawType = DebugDrawComponent.DrawType;

public class DebugGraphDraw
{
    private MonoBehaviour m_owner;            //���̃N���X�̏��L��
    public MonoBehaviour Owner => m_owner;

    readonly private GraphType m_graph;    //�O���t

    private GameObject m_nodeParentObject;
    private GameObject m_edgeParentObject;

    private List<GameObject> m_nodes = new List<GameObject>();  //�f�o�b�O�p�̃m�[�h
    private List<GameObject> m_edges = new List<GameObject>();  //�f�o�b�O�p�̃G�b�W

    public DebugGraphDraw(MonoBehaviour owner, GraphType graph)
    {
        m_owner = owner;
        m_graph = graph;

        m_nodeParentObject = new GameObject("DebugNodes");
        m_edgeParentObject = new GameObject("DeubgEdges");
    }

    public void CreateDebugNodes(GameObject prefab, Vector3? scale = null, DrawType drawType = DrawType.Cube, Color? color = null)
    {
        foreach (var node in m_graph.GetNodes())
        {
            var drawObject = Object.Instantiate(prefab, node.GetPosition(), Quaternion.identity, m_nodeParentObject.transform);
            //�X�P�[���ݒ�
            if (scale != null) {
                drawObject.transform.localScale = (Vector3)scale;
            }

            //�h���[�^�C�v��ݒ肷��B
            var debugDrawComponent = drawObject.GetComponent<DebugDrawComponent>();
            if (debugDrawComponent) {
                debugDrawComponent.drawType = drawType;
                if (color != null) {
                    debugDrawComponent.GizmosColor = (Color)color;
                }
            }
            
            m_nodes.Add(drawObject);
        }
    }

    public void CreateDebugEdges(GameObject prefab, Color? color = null)
    {
        foreach(var pair in m_graph.GetEdgesMap())
        {
            foreach(var edge in pair.Value)
            {
                var fromNode = m_graph.GetNode(edge.GetFromIndex());
                var toNode = m_graph.GetNode(edge.GetToIndex());

                var toNodeVec = toNode.GetPosition() - fromNode.GetPosition();
                var halfRange = toNodeVec.magnitude * 0.5f;  //�G�b�W�Ԃ̋����̔���
                var position = (fromNode.GetPosition() + toNode.GetPosition()) / 2.0f;

                var drawObject = Object.Instantiate(prefab, position, Quaternion.identity, m_edgeParentObject.transform);
                drawObject.transform.localScale = new Vector3(0.25f, 0.0f, halfRange);  //�X�P�[���ݒ�
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
    ///	�A�N�Z�b�T
    //--------------------------------------------------------------------------------------

}
