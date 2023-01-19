using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace StateNode
{
    public class AstarSeekTarget : EnemyStateNodeBase<EnemyBase>
    {
        private AstarSeek m_astarSeek;                          //Astar���g�����Ǐ]
        private TargetManager m_targetManager;                  //�^�[�Q�b�g�Ǘ�
        private SelfAstarNodeController m_selfNodeController;   //�������g�̃m�[�h�R���g���[���[

        private GameTimer m_timer;  //���ԊǗ��R���|�[�l���g

        private AstarSeek.Parametor m_seekParam;    //Astar�𗘗p�����Ǐ]�s��
        private float m_seekTime;                   //�Ǐ]����

        public AstarSeekTarget(EnemyBase owner) :
            this(owner, AstarSeek.DEFAULT_PARAMETOR, 10.0f)
        { }

        public AstarSeekTarget(EnemyBase owner, AstarSeek.Parametor seekParam, float seekTime) :
            base(owner)
        {
            m_seekParam = seekParam;
            m_seekTime = seekTime;
            m_astarSeek = owner.GetComponent<AstarSeek>();
            m_targetManager = owner.GetComponent<TargetManager>();
            m_selfNodeController = owner.GetComponent<SelfAstarNodeController>();

            m_timer = new GameTimer(0);
        }

        protected override void ReserveChangeComponents()
        {
            base.ReserveChangeComponents();

            AddChangeComp(GetOwner().GetComponent<AstarSeek>(), true, false);
        }

        public override void OnStart()
        {
            base.OnStart();

            m_astarSeek.SetParametor(m_seekParam);  //�Ǐ]�p�����[�^��ݒ�

            StartAstar();                           //Astar�Ǐ]�̏����Z�b�e�B���O

            m_timer.ResetTimer(m_seekTime);         //�^�C�}�[�v���J�n

            Debug.Log("��Astar�J�n");
        }

        public override bool OnUpdate()
        {
            if (m_astarSeek == null) {
                return true;
            }

            if (m_astarSeek.IsEnd()) {
                Debug.Log("��Astar�J�n");
                StartAstar();
            }

            m_timer.UpdateTimer();

            return IsEnd();
        }

        public override void OnExit()
        {
            Debug.Log("��Astar�I��");
            base.OnExit();
        }

        private void StartAstar()
        {
            if (!m_astarSeek) {
                return;
            }

            var aiDirector = AIDirector.Instance;
            var wayPointsMap = aiDirector.GetWayPointsMap();
            var selfNode = m_selfNodeController.GetNode();
            var targetNode = GetTargetNode();

            m_astarSeek.StartAstar(selfNode, targetNode, wayPointsMap.GetGraph());
            //m_astarSeek.StartAstar(selfNode, m_targetManager.GetCurrentTarget().transform.position, wayPointsMap.GetGraph(), 2);
        }

        private AstarNode GetTargetNode() {
            var target = m_targetManager.GetCurrentTarget();
            if (!target) {
                return null;
            }

            var nodeController = target.GetComponent<SelfAstarNodeController>();
            if (!nodeController) {
                return null;
            }

            return nodeController.GetNode();
        }

        private bool IsEnd() { return m_timer.IsTimeUp; }
    }
}

