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

        public override bool OnUpdate()
        {
            //敵の監視

            return false;
        }
    }
}
