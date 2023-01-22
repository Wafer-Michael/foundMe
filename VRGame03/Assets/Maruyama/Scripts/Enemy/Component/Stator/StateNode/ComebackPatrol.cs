using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace StateNode
{
    public class ComebackPatrol : EnemyStateNodeBase<EnemyBase>
    {
        private AutoMover m_autoMover;
        private AstarSeek m_astarSeek;
        private SelfAstarNodeController m_selfAstar;

        public ComebackPatrol(EnemyBase owner) :
            base(owner)
        {
            m_autoMover = owner.GetComponent<AutoMover>();
            m_astarSeek = owner.GetComponent<AstarSeek>();
            m_selfAstar = owner.GetComponent<SelfAstarNodeController>();
        }

        protected override void ReserveChangeComponents()
        {
            base.ReserveChangeComponents();

            AddChangeComp(GetOwner().GetComponent<AstarSeek>(), true, false);
        }

        public override void OnStart()
        {
            base.OnStart();

            var wayPointsMap = AIDirector.Instance.GetWayPointsMap();
            var factoryParam = AIDirector.Instance.GetFieldWayPointsMap_FactoryParametor();
            var targetPosition = m_autoMover.GetFirstPosition();
            m_astarSeek.StartAstar(m_selfAstar.GetNode(), targetPosition, wayPointsMap.GetGraph(), factoryParam.intervalRange);
        }

        public override bool OnUpdate()
        {
            return IsEnd();
        }

        public override void OnExit()
        {
            base.OnExit();
        }

        public bool IsEnd()
        {
            return m_astarSeek.IsEnd();

            var targetPosition = m_autoMover.GetFirstPosition();
            var range = (targetPosition - GetOwner().transform.position).magnitude;

            const float Range = 2.0f;
            if(range <= Range) {
                return true;
            }

            return false;
        }

    }

}

