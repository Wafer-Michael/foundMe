using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using System.Linq;

namespace StateNode
{
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

        private Cell m_currentCell = null;

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
            m_selfImpactCellController = owner.GetComponent<SelfImpactCellController>();

            Debug.Log("��LostPatrol");
        }

        protected override void ReserveChangeComponents()
        {
            base.ReserveChangeComponents();

            var owner = GetOwner();
            AddChangeComp(owner.GetComponent<AstarSeek>(), true, false);
        }

        public override void OnStart()
        {
            base.OnStart();

            //AstarSeek�̃p�����[�^��ݒ�
            m_timer.ResetTimer(m_param.time);
            m_astarSeek.SetParametor(m_param.seekParam);

            StartAstar();   //Astar�̊J�n
        }

        public override bool OnUpdate()
        {
            Debug.Log("��LostPatrol");

            m_timer.UpdateTimer();      //���Ԍv��

            if (m_astarSeek.IsEnd()) {  //AstarSeek���I��������
                StartAstar();
            }

            return IsEnd();              //��莞�Ԃ�������A�T�����I������B
        }

        private void StartAstar()
        {
            StartAstar_WayPoints();
        }

        private void StartAstar_CellMap()
        {
            //�f�o�b�O�悤�ɂ܂��c���Ă�����
            var factoryParametor = AIDirector.Instance.GetFieldWayPointsMap_FactoryParametor();
            var wayPointsMap = AIDirector.Instance.GetWayPointsMap().GetGraph();
            var selfNode = m_selfAstarNodeController.GetNode();
            var targetPosition = CalculateTargetPosition();

            //Astar�̊J�n
            m_astarSeek.StartAstar(selfNode, targetPosition, wayPointsMap, factoryParametor.intervalRange);
        }

        private void StartAstar_WayPoints() {
            var wayPointsMap = AIDirector.Instance.GetWayPointsMap().GetGraph();
            var selfNode = m_selfAstarNodeController.GetNode();
            var targetNode = CalculateTargetNode();

            m_astarSeek.StartAstar(selfNode, targetNode, wayPointsMap);
        }

        private AstarNode CalculateTargetNode()
        {
            AstarNode result = null;

            var wayPointsMap = AIDirector.Instance.GetWayPointsMap();       //�E�F�C�|�C���g�}�b�v
            var selfNode = m_selfAstarNodeController.GetNode();             //���݂̃m�[�h

            //���X�g�����ꏊ�t�߂Ŋ댯�x������(�܂��m�F���s���Ă��Ȃ�)�ꏊ���擾����B
            var openDatas = new Queue<AstarNode>();
            openDatas.Enqueue(selfNode);
            var closeDatas = new Queue<AstarNode>();

            //openData����ɂȂ�܂ŏ����𑱂���B
            while (openDatas.Count != 0)
            {
                var curretNode = openDatas.Dequeue();
                closeDatas.Enqueue(curretNode);

                var edges = wayPointsMap.GetGraph().GetEdges(curretNode.GetIndex());
                
                foreach (var edge in edges)
                {
                    //�ǉ�OpenData�ɒǉ��ł��邩�𔻒f
                    if (IsAddOpenData_AstarNode(openDatas, closeDatas, edge.GetToNode() as AstarNode))
                    {
                        openDatas.Enqueue(edge.GetToNode() as AstarNode);
                    }
                }
            }

            //�N���[�Y�f�[�^�����݂��Ȃ��Ȃ�A�������~
            if (closeDatas.Count == 0) {
                return result;
            }

            //�I�[�v���f�[�^�̒��ň�Ԋ댯�x�̍����ꏊ������
            var sortCloseDatas = closeDatas.OrderByDescending(value => { return value.GetDangerValue(); });
            result = sortCloseDatas.ToArray()[0];

            return result;
        }

        private Vector3 CalculateTargetPosition()
        {
            var result = Vector3.zero;

            var cellMap = AIDirector.Instance.GetImpactCellMap();       //�Z���}�b�v
            var selfCell = m_selfImpactCellController.GetCurrentCell(); //���݂̃Z��

            //���X�g�����ꏊ�t�߂Ŋ댯�x������(�܂��m�F���s���Ă��Ȃ�)�ꏊ���擾����B
            var openDatas = new Queue<ImpactCell>();
            openDatas.Enqueue(selfCell);
            var closeDatas = new Queue<ImpactCell>();

            //openData����ɂȂ�܂ŏ����𑱂���B
            while (openDatas.Count != 0)
            {
                var currentCell = openDatas.Dequeue();
                closeDatas.Enqueue(currentCell);

                //�������̃m�[�h���擾����B
                var cells = cellMap.FindEightDirectionCells(currentCell.GetIndex());

                foreach (var cell in cells)
                {
                    //�ǉ�OpenData�ɒǉ��ł��邩�𔻒f
                    if (IsAddOpenData(openDatas, closeDatas, cell)) {
                        openDatas.Enqueue(cell);
                    }
                }
            }

            //�N���[�Y�f�[�^�����݂��Ȃ��Ȃ�A�������~
            if (closeDatas.Count == 0) {
                return result;
            }

            //�I�[�v���f�[�^�̒��ň�Ԋ댯�x�̍����ꏊ������
            var sortCloseDatas = closeDatas.OrderByDescending(value => { return value.GetDangerValue(); });
            result = sortCloseDatas.ToArray()[0].GetPosition();

            //�f�o�b�O
            if (m_currentCell != null) {
                m_currentCell.IsTarget = false;
            }
            m_currentCell = sortCloseDatas.ToArray()[0];
            m_currentCell.IsTarget = true;

            return result;
        }

        private bool IsAddOpenData_AstarNode(Queue<AstarNode> openDatas, Queue<AstarNode> closeDatas, AstarNode node) {
            if (!node.IsActive()) {
                return false;
            }

            //��苗���ȏ�Ȃ珈�������Ȃ��B
            const float Range = 5.0f;
            var range = (node.GetPosition() - GetOwner().transform.position).magnitude;
            if(range > Range) {
                return false;
            }

            if (openDatas.Contains(node)) {
                return false;
            }

            if (closeDatas.Contains(node)) {
                return false;
            }

            return true;
        }

        private bool IsAddOpenData(Queue<ImpactCell> openDatas, Queue<ImpactCell> closeDatas, ImpactCell currentCell)
        {
            if (!currentCell.IsActive()) {
                return false;
            }

            //��苗���ȏ�Ȃ�ǉ����Ȃ�
            const float Range = 5.0f;
            var range = (currentCell.GetPosition() - GetOwner().transform.position).magnitude;
            if (range > Range)
            {
                return false;
            }

            if (openDatas.Contains(currentCell))
            {
                return false;
            }

            if (closeDatas.Contains(currentCell))
            {
                return false;
            }

            //��둤�Ȃ珈�������Ȃ��B

            return true;
        }

        private bool IsEnd() { return m_timer.IsTimeUp; }
    }

}
