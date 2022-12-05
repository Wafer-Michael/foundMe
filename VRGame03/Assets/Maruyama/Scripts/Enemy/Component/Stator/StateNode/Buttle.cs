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

        private TaskList<TaskEnum> m_taskList = new TaskList<TaskEnum>();

        public Buttle(EnemyBase owner) :
            base(owner)
        {
            m_targetManager = owner.GetComponent<TargetManager>();
            m_velocityManager = owner.GetComponent<VelocityManager>();

            DefineTask();
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

                var damaged = target.GetComponent<I_Damaged>();
                damaged?.Damaged(new DamageData(1.0f));
            };

            m_taskList.DefineTask(TaskEnum.Attack, attackFunc, null, null);

            m_taskList.DefineTask(TaskEnum.Wait, new TaskNode.Task_Wait(3.0f));
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
