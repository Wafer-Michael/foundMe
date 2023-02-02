using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using System.Linq;

using GraphType = SparseGraph<AstarNode, AstarEdge>;

public class OpenData
{
    public AstarNode parent;    //�����̑O�̃m�[�h
    public AstarNode node;      //�������g�̃m�[�h�|�C���^
    public float range;         //������
    public float heuristic;     //�q���[�X���b�N����
    public bool isOpen;

    public OpenData(AstarNode parent, AstarNode selfNode, float range, float heuristic)
    {
        this.parent = parent;
        this.node = selfNode;
        this.range = range;
        this.heuristic = heuristic;
        this.isOpen = true;
    }

    public float GetSumRange() { return range + heuristic; }

    public bool IsEqual(OpenData other)
    {
        return (this.parent == other.parent &&
                this.node == other.node);
    }

    public void move(OpenData other)
    {
        this.parent = other.parent;
        this.node = other.node;
        this.range = other.range;
        this.heuristic = other.heuristic;
        this.isOpen = other.isOpen;
    }
}

public class OpenDataHelper
{
    private Stack<AstarNode> m_route;   //�����������[�g

    private AstarNode m_otherAreaNode;  //�ʃG���A�m�[�h

    public OpenDataHelper()
    {
        m_route = new Stack<AstarNode>();
        m_otherAreaNode = null;
    }

    /// <summary>
    /// �I�[�v���f�[�^�𐶐����邽�߂̊�ƂȂ�I�[�v���f�[�^������(��Ԋ��Ғl�̍����m�[�h���擾����B)
    /// </summary>
    /// <returns>�I�[�v���f�[�^�𐶐����邽�߂̊J�n�m�[�h</returns>
    private OpenData FindBaseOpenData(List<OpenData> openDatas)
    {
        var sortDatas = openDatas.OrderBy(value => { return value.GetSumRange(); });

        return sortDatas.ToArray()[0];
    }

    private bool IsOtherAreaTarget(AstarNode startNode, int targetAreaIndex)
    {
        //�ڕW�G���A��0�ȏォ�A�ڕW�m�[�h���ڕW�G���A�ƈႤ�Ƃ�
        return (targetAreaIndex >= 0 && targetAreaIndex != startNode.GetParent().GetIndex());
    }

    private bool CreateOpenDatas(
        ref List<OpenData> openDatas,
        ref List<OpenData> closeDatas,
        OpenData openData,
        GraphType graph,
        AstarNode startNode,
        AstarNode targetNode,
        int targetAreaIndex = -1
    ) {
        var baseNode = openData.node;
        var edges = graph.GetEdges(baseNode.GetIndex());
        int baseIndex = baseNode.GetIndex();

        openDatas.Remove(openData); //�g�p����I�[�v���f�[�^�����X�g����폜
        closeDatas.Add(openData);   //�g�p����I�[�v���f�[�^���N���[�Y���X�g�ɒǉ�

        var otherAreaOpenDatas = new List<OpenData>();

        if(edges == null) {
            return false;
        }

        foreach (var edge in edges)
        {
            //var node = graph.GetNode(edge.GetToIndex());    //�m�[�h�̎擾
            var node = edge.GetToNode() as AstarNode;
            if (node == null) {
                continue;
            }

            var toNodeVec = node.GetPosition() - baseNode.GetPosition();
            float range = toNodeVec.magnitude;
            range += openData.range;
            float heuristicRange = (targetNode.GetPosition() - node.GetPosition()).magnitude;

            int newIndex = node.GetIndex();
            var newData = new OpenData(baseNode, node, range, heuristicRange);

            bool isAddData = AddOpenData(openDatas, closeDatas, newData);

            //�ڕW�m�[�h�Ȃ�I��点��
            if(node == targetNode) {
                return true;
            }

            //�ڕW�G���A�����G���A�ŁA���A�ڕW�G���A�̃m�[�h�Ȃ�otherAreaOpenDatas�ɓo�^����
            if (IsOtherAreaTarget(startNode, targetAreaIndex) && node.GetParent().GetIndex() == targetAreaIndex)
            {
                otherAreaOpenDatas.Add(newData);
            }
        }

        //�ʃG���A�̃f�[�^�����݂���Ȃ�A�ʃG���A�Ƃ̋��ڂŒT���I��
        if(otherAreaOpenDatas.Count != 0) {
            var sotrDatas = otherAreaOpenDatas.OrderBy(value => value.GetSumRange());
            m_otherAreaNode = sotrDatas.ToArray()[0].node;
            //return true;  //����G���A���Ƃ̕ύX�����܂����������f�ł��Ȃ�����ۗ�
        }

        return false;
    }

    private bool CreateRoute(List<OpenData> openDatas, AstarNode targetNode)
    {
        int index = 0;
        const int TempMaxIndex = 100000;

        var tempData = FindSomeNodeOpenData(openDatas, targetNode);

        while(index <= TempMaxIndex)
        {
            if (tempData.parent == null) {  //�e�m�[�h�����݂��Ȃ��Ȃ�A�������I��
                break;
            }

            m_route.Push(tempData.node);
            tempData = FindSomeNodeOpenData(openDatas, tempData.parent);
            index++;
        }

        return index <= TempMaxIndex;
    }

    private OpenData FindSomeNodeOpenData(List<OpenData> openDatas, AstarNode node)
    {
        foreach (var data in openDatas)
        {
            if(data.node == node) {
                return data;
            }
        }

        return null;
    }

    private bool IsRegisterData(List<OpenData> openDatas, OpenData openData)
    {
        foreach(var data in openDatas)
        {
            if(data.IsEqual(openData)) {
                return true;
            }
        }

        return false;
    }

    private bool AddOpenData(
        List<OpenData> openDatas, 
        List<OpenData> closeDatas, 
        OpenData newData
    ) {
        var someOpenData = FindSomeNodeOpenData(openDatas, newData.node);
        var someCloseData = FindSomeNodeOpenData(closeDatas, newData.node);
        
        //�I�[�v�����X�g�ɓo�^����Ă��Ȃ��A���A�N���[�Y���X�g�ɓo�^����Ă��Ȃ�
        if(someOpenData == null && someCloseData == null)
        {
            openDatas.Add(newData);
            return true;
        }

        //�I�[�v�����X�g�ɓo�^����Ă��āA�V�K�f�[�^�̕������������v�l
        if (someOpenData != null && IsSmall_LeftOpenData(someOpenData, newData))
        {
            someOpenData.move(newData);
            return true;
        }

        //�N���[�Y���X�g�ɓo�^����Ă��āA�V�K�f�[�^�̕������������v�l
        if(someCloseData != null && !IsSmall_LeftOpenData(someCloseData, newData))
        {
            openDatas.Add(newData);
            closeDatas.Remove(someCloseData);
            return true;
        }

        //�ǂ̏����ɂ����Ă͂܂�Ȃ��Ȃ�A�ǉ������Ȃ�
        return false;
    }

    public bool StartSearch(
        AstarNode startNode,
        AstarNode targetNode,
        GraphType graph,
        int targetAreaIndex = -1
    ) {
        m_route.Clear();
        m_otherAreaNode = null;

        //�I�[�v���f�[�^���X�g�ƃN���[�Y�f�[�^���X�g���쐬
        var openDatas = new List<OpenData>();
        var closeDatas = new List<OpenData>();

        //�����f�[�^�̐���
        openDatas.Add(new OpenData(null, startNode, 0.0f, CalculateHeuristicRange(startNode, targetNode)));

        //�I�[�v���f�[�^�����݂�����胋�[�v����B
        while(openDatas.Count != 0)
        {
            //�I�[�v���f�[�^�����p�̊�m�[�h�̐���
            var baseOpenData = FindBaseOpenData(openDatas);

            //�I�[�v���f�[�^�̐����B�^�[�Q�b�g�m�[�h�ɂ��ǂ蒅������true��Ԃ��B
            if(CreateOpenDatas(ref openDatas, ref closeDatas, baseOpenData, graph, startNode, targetNode)) {
                break;
            }
        }

        bool isAstarSuccess = (openDatas.Count != 0);  //�I�[�v���f�[�^�����݂���Ȃ�A�������s

        if (!isAstarSuccess) {  //���s�����玟�̏������΂��B
            return isAstarSuccess;
        }

        //�N���[�Y�f�[�^���I�[�v���f�[�^�Ɋ܂߂�B
        foreach(var closeData in closeDatas)
        {
            openDatas.Add(closeData);
        }

        //var lastTargetNode = m_otherAreaNode != null ? m_otherAreaNode : targetNode;
        var lastTargetNode = targetNode;
        var isCreateRoute = CreateRoute(openDatas, lastTargetNode);
        if (!isCreateRoute) {
            Debug.Log("���[�g�����Ɏ��s���܂����B");
        }

        return isAstarSuccess;
    }

    public Stack<AstarNode> GetRoute() { return m_route; }

    private bool IsSmall_LeftOpenData(OpenData left, OpenData right)
    {
        return left.GetSumRange() < right.GetSumRange();
    }

    private float CalculateHeuristicRange(AstarNode targetNode, AstarNode selfMode)
    {
        return (targetNode.GetPosition() - selfMode.GetPosition()).magnitude;
    }
}
