using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace StateNode
{
    public class NormalSeekTarget : EnemyStateNodeBase<EnemyBase>
    {
        public struct Parametor {
            public float maxSpeed;
            public float turingPower;
        }

        Parametor m_param;

        private TargetManager m_targetManager;           //ターゲット監視
        private VelocityManager m_velocityManager;      
        private RotationController m_rotationController;

        public NormalSeekTarget(EnemyBase owner) :
            base(owner)
        {
            m_targetManager = owner.GetComponent<TargetManager>();
            m_velocityManager = owner.GetComponent<VelocityManager>();
            m_rotationController = owner.GetComponent<RotationController>();

            //仮パラメータ
            m_param.maxSpeed = 2.0f;
            m_param.turingPower = 1.0f;
        }

        public override void OnStart()
        {
            base.OnStart();

            Debug.Log("▲Start_NormalSeek");
        }

        public override bool OnUpdate()
        {
            Debug.Log("▲NormalSeek");

            MoveUpdate();

            return false;
        }

        private void MoveUpdate()
        {
            var target = m_targetManager.GetCurrentTarget();
            if (!target) {
                return;
            }

            var toVec = target.transform.position - GetOwner().transform.position;
            float maxSpeed = m_param.maxSpeed;
            float turningPower = m_param.turingPower;
            Vector3 force = maru.CalculateVelocity.SeekVec(m_velocityManager.velocity, toVec, maxSpeed);
            m_velocityManager.AddForce(force * turningPower);

            m_rotationController.SetDirection(m_velocityManager.velocity);
        }
    }
}