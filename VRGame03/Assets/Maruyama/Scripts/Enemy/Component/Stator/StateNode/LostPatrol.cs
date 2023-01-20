using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// player�������������̜p�j�X�e�[�g
/// </summary>
public class LostPatrol : EnemyStateNodeBase<EnemyBase>
{
    private enum TaskEnum
    {
        
    }

    public struct Parametor 
    {
        public float time;                      //�T��������ő厞��
        public AstarSeek.Parametor seekParam;   //�Ǐ]�p�����[�^
    }

    private Parametor m_param;  //�p�����[�^

    private GameTimer m_timer;  //�^�C�}�[�Ǘ��N���X

    private TargetManager m_targetManager;                          //�^�[�Q�b�g�Ǘ�
    private AstarSeek m_astarSeek;                                  //AstarSeek�̐ݒ�
    private SelfAstarNodeController m_selfAstarNodeController;      //�������g����������AstarNode�̌���
    private SelfImpactCellController m_selfImpactCellController;    //��������������Z�����Ǘ�����B

    private TaskList<TaskEnum> m_taskList = new TaskList<TaskEnum>();

    public LostPatrol(EnemyBase owner) :
        this(owner, new Parametor() { time = 10.0f, seekParam = AstarSeek.DEFAULT_PARAMETOR })
    { }

    public LostPatrol(EnemyBase owner, Parametor parametor) :
        base(owner)
    {
        m_param = parametor;
        m_timer = new GameTimer();

        m_targetManager = owner.GetComponent<TargetManager>();
        m_astarSeek = owner.GetComponent<AstarSeek>();
        m_selfAstarNodeController = owner.GetComponent<SelfAstarNodeController>();
    }

    protected override void ReserveChangeComponents()
    {
        base.ReserveChangeComponents();

        var owner = GetOwner();
        AddChangeComp(owner, true, false);
    }

    public override void OnStart()
    {
        base.OnStart();

        //AstarSeek�̃p�����[�^��ݒ�
        m_astarSeek.SetParametor(m_param.seekParam);

        StartAstar();   //Astar�̊J�n
    }

    public override bool OnUpdate()
    {   
        m_timer.UpdateTimer();      //���Ԍv��

        if (m_astarSeek.IsEnd()) {  //AstarSeek���I��������
            StartAstar();
        }

        return IsEnd();     //��莞�Ԃ�������A�T�����I������B
    }

    private void StartAstar()
    {
        var factoryParametor = AIDirector.Instance.GetFieldWayPointsMap_FactoryParametor();
        var wayPointsMap = AIDirector.Instance.GetWayPointsMap().GetGraph();
        var selfNode = m_selfAstarNodeController.GetNode();
        var targetPosition = CalculateTargetPosition();

        //Astar�̊J�n
        m_astarSeek.StartAstar(selfNode, targetPosition, wayPointsMap, factoryParametor.intervalRange);
    }

    private Vector3 CalculateTargetPosition()
    {
        var result = Vector3.zero;

        var wayPointsMap = AIDirector.Instance.GetWayPointsMap();   //�E�F�C�|�C���g�}�b�v
        var selfNode = m_selfAstarNodeController.GetNode();         //��������������J�n�m�[�h

        //���X�g�����ꏊ�t�߂Ŋ댯�x������(�܂��m�F���s���Ă��Ȃ�)�ꏊ���擾����B
        var openDatas = new Queue<AstarNode>();
        openDatas.Enqueue(selfNode);

        //openData����ɂȂ�܂ŏ����𑱂���B
        while (openDatas.Count != 0) {
            //�������̃m�[�h���擾����B

        }

        return result;
    }

    private bool IsEnd() { return m_timer.IsTimeUp; }
}
