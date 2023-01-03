using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace StateNode
{
    public class AstarSeekTarget : EnemyStateNodeBase<EnemyBase>
    {
        private AstarSeek m_astarSeek;  //AstarÇégÇ¡ÇΩí«è]
        private TargetManager m_targetManager;
        private SelfAstarNodeController m_selfNodeController;

        public AstarSeekTarget(EnemyBase owner):
            base(owner)
        {
            m_astarSeek = owner.GetComponent<AstarSeek>();
            m_targetManager = owner.GetComponent<TargetManager>();
            m_selfNodeController = owner.GetComponent<SelfAstarNodeController>();
        }

        protected override void ReserveChangeComponents()
        {
            base.ReserveChangeComponents();

            AddChangeComp(GetOwner().GetComponent<AstarSeek>(), true, false);
        }

        public override void OnStart()
        {
            base.OnStart();

            if (m_astarSeek) {
                var aiDirector = AIDirector.Instance;
                var wayPointsMap = aiDirector.GetWayPointsMap();
                var selfNode = m_selfNodeController.GetNode();
                var targetNode = GetTargetNode();
                m_astarSeek.StartAstar(selfNode, targetNode, wayPointsMap.GetGraph());
            }
        }

        public override bool OnUpdate()
        {
            if(m_astarSeek == null) {
                return true;
            }

            return m_astarSeek.IsEnd();
        }

        public override void OnExit()
        {
            base.OnExit();
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
    }
}

