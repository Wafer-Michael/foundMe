using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using System.Linq;
using System.Threading;
using System.Threading.Tasks;

public class SelfAstarNodeController : MonoBehaviour
{
    private AstarNode m_node = null;

    private bool m_isInitialize = false;    //�����������ǂ���

    [SerializeField]
    private bool m_isMover = true;  //�ړ����邽�߁A��ɊĎ����K�v���ǂ���

    private void Start()
    {
        InitializeNode();
    }

    private void Update()
    {
        //�ړ�����҂̂ݍX�V������������B
        if (IsMover()) {
            UpdateProcess();
        }

        if (HasNode()) {
            //Debug.Log("��" + GetNode().GetIndex().ToString());
        }
    }

    private void UpdateProcess()
    {
        if (IsInitialize()) {   //���������Ȃ珈�����΂�
            return;
        }

        //�m�[�h���Ȃ��A���́A���݂̃m�[�h���炩�Ȃ藣��Ă���ꍇ
        if(!HasNode() || IsFarRange()) {
            InitializeNode();
            return;
        }

        //�m�[�h�̍X�V���K�v�Ȃ�
        if (IsUpdateNode()) {
            UpdateNode();
        }
    }

    /// <summary>
    /// �m�[�h�̏�����
    /// </summary>
    private void InitializeNode()
    {
        if (IsInitialize()) {   //���������Ȃ�Ă΂Ȃ��B
            return;
        }

        var position = transform.position;
        _ = Task.Run(() => InitializeProcess(position));    //�ĂԂ����ǂ�ŕ��u�}���`�X���b�h(�����I�ɃX���b�h�v�[���ɕύX)
    }

    private void InitializeProcess(Vector3 position)
    {
        m_isInitialize = true;
        var wayPointsMap = GetWayPointsMap();

        //����
        m_node = maru.UtilityAstar.FindNearAstarNode(wayPointsMap.GetGraph(), position);

        m_isInitialize = false;

        Debug.Log("�������I��");
    }

    private void UpdateNode()
    {
        //�m�[�h�������ĂāA�����Ȃ��Ȃ�A
        if (HasNode() && !IsMover()) {
            return;
        }

        if (!HasNode()) {
            InitializeNode();
            return;
        }

        var edges = GetWayPointsMap().GetGraph().GetEdges(m_node.GetIndex());
        if(edges == null) {
            InitializeNode();
            return;
        }

        if(edges.Count == 0) {
            InitializeNode();
            return;
        }

        //��ԋ߂��m�[�h���Ƀ\�[�g
        var sotrEdges = edges.OrderBy(value => {
            var toNode = value.GetToNode() as AstarNode;
            return (toNode.GetPosition() - transform.position).magnitude;
        });

        var nextNode = sotrEdges.ToList()[0].GetToNode() as AstarNode;

        m_node = nextNode;
    }

    private bool IsUpdateNode()
    {
        if (!HasNode()) {   //�m�[�h�������Ă��Ȃ��Ȃ�Afalse
            return false;
        }

        var wayPointsMap = GetWayPointsMap();
        var intervalRange = wayPointsMap.GetIntervalRange();    //�m�[�h�Ԃ̋������擾

        float range = (m_node.GetPosition() - transform.position).magnitude;

        return intervalRange < range;
    }

    private bool IsFarRange() {
        if (!HasNode()) {   //�m�[�h�������Ă��Ȃ��Ȃ�Afalse
            return false;
        }

        var wayPointsMap = GetWayPointsMap();
        var intervalRange = wayPointsMap.GetIntervalRange();    //�m�[�h�Ԃ̋������擾
        const float FarCost = 1.0f; //���̃R�X�g(�����I�ɕύX)
        float farRange = intervalRange + FarCost;

        float range = (m_node.GetPosition() - transform.position).magnitude;

        return farRange < range;   //�m�[�h��苗�������ꂽ�ꍇ
    }

    //--------------------------------------------------------------------------------------
    /// �A�N�Z�b�T
    //--------------------------------------------------------------------------------------

    private void SetNode(AstarNode node) { m_node = node; }

    public AstarNode GetNode() { return m_node; }

    public bool HasNode() { return m_node != null; }

    private WayPointsMap GetWayPointsMap() { return AIDirector.Instance.GetWayPointsMap(); }

    public bool IsInitialize() { return m_isInitialize; }

    public bool IsMover() { return m_isMover; }

    public bool HasCurrentNode() { return m_node != null; }

}
