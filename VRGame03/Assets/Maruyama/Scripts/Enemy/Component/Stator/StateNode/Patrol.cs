using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace StateNode
{
    public class Patrol : EnemyStateNodeBase<EnemyBase>
    {
        private TargetManager m_targetManager;  //ターゲット監視

        public Patrol(EnemyBase owner) :
            base(owner)
        {
            m_targetManager = owner.GetComponent<TargetManager>();
        }

        protected override void ReserveChangeComponents()
        {
            base.ReserveChangeComponents();

            var owner = GetOwner();
            AddChangeComp(owner.GetComponent<AutoMover>(), true, false);    //将来的に消す。
        }

        public override bool OnUpdate()
        {
            //敵の監視
            Debug.Log("Patrol");
            return false;
        }
    }
}
