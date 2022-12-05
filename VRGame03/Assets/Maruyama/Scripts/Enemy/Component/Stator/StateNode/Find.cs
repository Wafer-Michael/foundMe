using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace StateNode
{
    public class Find : EnemyStateNodeBase<EnemyBase>
    {
        private TargetManager m_targetManager;  //ターゲット監視

        public Find(EnemyBase owner) :
            base(owner)
        {
            m_targetManager = owner.GetComponent<TargetManager>();
        }

        public override bool OnUpdate()
        {
            //敵の監視
            Debug.Log("Find");
            return false;
        }
    }
}
