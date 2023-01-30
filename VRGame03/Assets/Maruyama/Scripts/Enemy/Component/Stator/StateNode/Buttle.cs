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
        private AttackAnimationController m_attackAnimation;

        private TaskList<TaskEnum> m_taskList = new TaskList<TaskEnum>();

        public Buttle(EnemyBase owner) :
            base(owner)
        {
            m_targetManager = owner.GetComponent<TargetManager>();
            m_velocityManager = owner.GetComponent<VelocityManager>();
            m_attackAnimation = owner.GetComponent<AttackAnimationController>();

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
            Debug.Log("šButtle");

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
            //—\”õ“®ì
            m_taskList.DefineTask(TaskEnum.Preliminary, new TaskNode.TacklePreliminary(GetOwner()));



            //UŒ‚
            //m_taskList.DefineTask(TaskEnum.Attack, new TaskNode.TackleAttack(GetOwner()));
            m_taskList.DefineTask(TaskEnum.Attack, () => m_attackAnimation.AttackStart(), null, null);

            //‘Ò‹@
            const float WaitTime = 3.0f;
            m_taskList.DefineTask(TaskEnum.Wait, new TaskNode.Task_Wait(WaitTime));
        }

        private void SelectTask()
        {
            TaskEnum[] tasks = { 
                //TaskEnum.Preliminary,
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
    class TacklePreliminary : TaskNodeBase<EnemyBase>
    { 
        public struct Parametor
        {
            public float speed;
            
        }

        public TacklePreliminary(EnemyBase owner):
            base(owner)
        { }

        public override void OnEnter()
        {
            base.OnEnter();
        }

        public override bool OnUpdate()
        {
            

            //var owner = GetOwner();
            //owner.transform.rotation = Quaternion.Lerp(owner.transform.rotation,
            //             Quaternion.LookRotation(direct),
            //             m_rotationSpeed * Time.deltaTime);

            return IsEnd();
        }

        public override void OnExit()
        {
            base.OnExit();
        }

        public bool IsEnd()
        {
            return true;
        }
    }

    //ƒ^ƒbƒNƒ‹UŒ‚
    class TackleAttack : TaskNodeBase<EnemyBase>
    {
        public TackleAttack(EnemyBase owner) :
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