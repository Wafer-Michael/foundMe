using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace StateNode
{
    public class Buttle : EnemyStateNodeBase<EnemyBase>
    {
        private enum TaskEnum {
            Attack,
            Wait,
        }

        private TargetManager m_targetManager;  //�^�[�Q�b�g�Ď�
        private VelocityManager m_velocityManager;
        private AttackAnimationController m_attackAnimationController;

        private TaskList<TaskEnum> m_taskList = new TaskList<TaskEnum>();

        public Buttle(EnemyBase owner) :
            base(owner)
        {
            m_targetManager = owner.GetComponent<TargetManager>();
            m_velocityManager = owner.GetComponent<VelocityManager>();
            m_attackAnimationController = owner.GetComponent<AttackAnimationController>();

            DefineTask();
        }

        protected override void ReserveChangeComponents()
        {
            base.ReserveChangeComponents();

            var owner = GetOwner();

            //AddChangeComp(owner.GetComponent<VelocityManager>(), false, true);
        }

        public override void OnStart()
        {
            base.OnStart();

            m_taskList.ForceStop();
            SelectTask();

            m_velocityManager.StartDeseleration();
        }

        public override bool OnUpdate()
        {
            //�G�̊Ď�
            Debug.Log("Buttle");

            m_taskList.UpdateTask();

            return m_taskList.IsEnd;
        }

        public override void OnExit()
        {
            base.OnExit();

            m_taskList.ForceStop();
            m_velocityManager.SetIsDeseleration(false);
        }

        private void DefineTask()
        {
            //���U��
            System.Action attackFunc = () =>
            {
                var target = m_targetManager.GetCurrentTarget();
                if (!target) {
                    return;
                }

                m_attackAnimationController.AttackStart();
            };

            m_taskList.DefineTask(TaskEnum.Attack, attackFunc, null, null);

            const float WaitTime = 3.0f;
            m_taskList.DefineTask(TaskEnum.Wait, new TaskNode.Task_Wait(WaitTime));
        }

        private void SelectTask()
        {
            TaskEnum[] tasks = { 
                TaskEnum.Attack,
                TaskEnum.Wait,
            };

            foreach(var task in tasks) {
                m_taskList.AddTask(task);
            }
        }
    }
}
