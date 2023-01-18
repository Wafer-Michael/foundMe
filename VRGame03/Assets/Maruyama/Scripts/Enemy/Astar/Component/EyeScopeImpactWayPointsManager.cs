using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EyeScopeImpactWayPointsManager : MonoBehaviour
{
    private EyeSearchRange m_eyeRange;

    private SelfAstarNodeController m_selfAstarNodeController;

    private void Awake()
    {
        m_eyeRange = GetComponent<EyeSearchRange>();
        m_selfAstarNodeController = GetComponent<SelfAstarNodeController>();
    }

    private void Update()
    {
        var nodes = FindEyeScopeNodes_FloodFill();
    }

    private Queue<AstarNode> FindEyeScopeNodes_FloodFill()
    {
        //���݈ʒu��Node�����݂��Ȃ��Ȃ珈�������Ȃ�
        if (!m_selfAstarNodeController.HasCurrentNode()) {
            return null;
        }

        AstarNode startCell = m_selfAstarNodeController.GetNode();
        var wayPointMap = AIDirector.Instance.GetWayPointsMap();

        var openNodes = new Queue<AstarNode>();
        var closeNodes = new Queue<AstarNode>();
        openNodes.Enqueue(startCell);

        while(openNodes.Count != 0)
        {
            var currentNode = openNodes.Dequeue();  //�F������Z�����擾
            closeNodes.Enqueue(currentNode);        //�N���[�Y���X�g�ɓo�^

            //�������̃Z�����擾
            var edges = wayPointMap.GetGraph().GetEdges(currentNode.GetIndex());

            foreach (var edge in edges)
            {
                var node = edge.GetToNode() as AstarNode;
                if (node == null) {
                    continue;
                }

                //�I�[�v���f�[�^�ɓo�^�ł��邩�ǂ���
                if (IsAddOpenNodes(node, openNodes, closeNodes)) {
                    openNodes.Enqueue(node);
                }
            }
        }

        return closeNodes;
    }

    private bool IsAddOpenNodes(AstarNode node, Queue<AstarNode> openNodes, Queue<AstarNode> closeNodes)
    {
        //���łɃI�[�v���f�[�^�ɓo�^����Ă���ꍇ
        if (openNodes.Contains(node))
        {
            return false;
        }

        //���łɃN���[�Y�f�[�^�ɓo�^����Ă���ꍇ
        if (closeNodes.Contains(node))
        {
            return false;
        }

        //���E���ɑ��݂��Ȃ��ꍇ�B
        if (!m_eyeRange.IsInEyeRange(node.GetPosition()))
        {
            return false;
        }

        return true;    //�S�Ă̏������N���A�������߁A���E���̃Z��
    }

}
