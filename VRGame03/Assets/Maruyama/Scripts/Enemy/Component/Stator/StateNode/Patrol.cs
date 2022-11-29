using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace StateNode
{
    public class Patrol : EnemyStateNodeBase<EnemyBase>
    {
        private TargetManager m_targetManager;  //�^�[�Q�b�g�Ď�

        public Patrol(EnemyBase owner) :
            base(owner)
        {
            m_targetManager = owner.GetComponent<TargetManager>();
        }

        public override bool OnUpdate()
        {
            //�G�̊Ď�
            Debug.Log("Patrol");
            return false;
        }
    }
}
