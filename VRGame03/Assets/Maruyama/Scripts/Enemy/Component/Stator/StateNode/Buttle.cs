using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace StateNode
{
    public class Buttle : EnemyStateNodeBase<EnemyBase>
    {
        private enum TaskEnum {
            Preliminary,    //—\”õ“®ì
            Attack,         //UŒ‚
            Wait,           //‘Ò‹@
        }

        private TargetManager m_targetManager;  //ƒ^[ƒQƒbƒgŠÄ‹
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
            //“G‚ÌŠÄ‹
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
            //‰¼UŒ‚
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


namespace TaskNode
{
    //—\”õ“®ì
    class TacklePreliminary : TaskNodeBase<GameObject>
    { 
        public TacklePreliminary(GameObject owner):
            base(owner)
        { }

        public override void OnEnter()
        {
            base.OnEnter();
        }

        public override bool OnUpdate()
        {
            return true;
        }

        public override void OnExit()
        {
            base.OnExit();
        }
    }

    //ƒ^ƒbƒNƒ‹UŒ‚
    class TackleAttack : TaskNodeBase<GameObject>
    {
        public TackleAttack(GameObject owner) :
            base(owner)
        { }

        public override void OnEnter()
        {
            base.OnEnter();
        }

        public override bool OnUpdate()
        {
            return true;
        }

        public override void OnExit()
        {
            base.OnExit();
        }
    }
    

}