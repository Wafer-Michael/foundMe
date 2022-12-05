using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using StateType = StateNode.Chase_StateType;
using TransitionMember = StateNode.Chase_TransitionMember;
using StateMachine = StateMachine<EnemyBase, StateNode.Chase_StateType, StateNode.Chase_TransitionMember>;

namespace StateNode
{
    public enum Chase_StateType
    {
        None,
        Normal,     //通常シーク
        BreadCrumb, //ブレッドクラム
    }

    public struct Chase_TransitionMember {
           
    }

    public class Chase : EnemyStateNodeBase<EnemyBase>
    {
        private float m_nearRange = 3.0f;
        private float m_maxSpeed = 2.0f;
        private float m_turningPower = 1.0f;  //旋回する力
        private float m_lostSeekTime = 10.0f; //ロストしてから一定時間追従する時間

        private TargetManager m_targetManager;  //ターゲット監視

        StateMachine m_stateMachine = new StateMachine();

        public Chase(EnemyBase owner) :
            base(owner)
        {
            m_targetManager = owner.GetComponent<TargetManager>();

            CreateNode();
            CreateEdge();
        }

        protected override void ReserveChangeComponents()
        {
            base.ReserveChangeComponents();

            var owner = GetOwner();
            AddChangeComp(owner.GetComponent<AutoMover>(), false, false);    //将来的に消す。
        }

        public override void OnStart()
        {
            base.OnStart();

            SettingStartTarget();    //ターゲットの設定。

            m_stateMachine.ChangeState(StateType.Normal, (int)StateType.Normal);

            Debug.Log("ChaseStart");
        }

        public override bool OnUpdate()
        {
            m_stateMachine.OnUpdate();

            return IsEnd();
        }

        public override void OnExit()
        {
            base.OnExit();

            m_stateMachine.ForceChangeState(StateType.None);
        }

        private void SettingStartTarget()
        {
            var target = GetOwner().GetObserveIsInEyeTargets().SerachNearIsInEyeTarget();
            m_targetManager.SetCurrentTarget(target);
        }

        void CreateNode()
        {
            m_stateMachine.AddNode(StateType.None, null);

            m_stateMachine.AddNode(StateType.Normal, new StateNode.NormalSeekTarget(GetOwner()));

            m_stateMachine.AddNode(StateType.BreadCrumb, new StateNode.BreadSeekTarget(GetOwner(), m_nearRange, m_maxSpeed, m_turningPower, m_lostSeekTime));
        }

        void CreateEdge()
        {
            
        }

        private bool IsEnd() {
            //ターゲットがいないなら終了
            return !m_targetManager.HasTarget(); 
        }   
    }
}
