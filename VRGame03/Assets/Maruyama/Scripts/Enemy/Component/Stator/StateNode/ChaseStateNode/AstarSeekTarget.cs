using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace StateNode
{
    public class AstarSeekTarget : EnemyStateNodeBase<EnemyBase>
    {
        private AstarSeek m_astarSeek;                          //Astarを使った追従
        private TargetManager m_targetManager;                  //ターゲット管理
        private SelfAstarNodeController m_selfNodeController;   //自分自身のノードコントローラー

        private GameTimer m_timer;  //時間管理コンポーネント

        private AstarSeek.Parametor m_seekParam;    //Astarを利用した追従行動
        private float m_seekTime;                   //追従時間

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

            m_astarSeek.SetParametor(m_seekParam);  //追従パラメータを設定

            StartAstar();                           //Astar追従の初期セッティング

            m_timer.ResetTimer(m_seekTime);         //タイマー計測開始

            Debug.Log("★Astar開始");
        }

        public override bool OnUpdate()
        {
            if (m_astarSeek == null) {
                return true;
            }

            if (m_astarSeek.IsEnd()) {
                StartAstar();
            }

            m_timer.UpdateTimer();

            return IsEnd();
        }

        public override void OnExit()
        {
            Debug.Log("★Astar終了");
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

