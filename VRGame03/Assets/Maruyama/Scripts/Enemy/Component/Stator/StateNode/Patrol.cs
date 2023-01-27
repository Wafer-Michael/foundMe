using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace StateNode
{
    public class Patrol : EnemyStateNodeBase<EnemyBase>
    {
        private TargetManager m_targetManager;      //�^�[�Q�b�g�Ď�
        private VelocityManager m_velocityManager;  //���x�Ǘ�

        public Patrol(EnemyBase owner) :
            base(owner)
        {
            m_targetManager = owner.GetComponent<TargetManager>();
            m_velocityManager = owner.GetComponent<VelocityManager>();
        }

        protected override void ReserveChangeComponents()
        {
            base.ReserveChangeComponents();

            var owner = GetOwner();
            AddChangeComp(owner.GetComponent<AutoMover>(), true, false);    //�����I�ɏ����B
        }

        public override void OnStart()
        {
            base.OnStart();

            m_velocityManager.ResetAll();
        }

        public override bool OnUpdate()
        {
            //�G�̊Ď�
            Debug.Log("Patrol");
            return false;
        }
    }
}
