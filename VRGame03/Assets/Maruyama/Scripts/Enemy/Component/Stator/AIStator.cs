using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using StateType = AIStator_StateType;
using TransitionMember = AIStator_TransitionMember;

public enum AIStator_StateType 
{
    None,
    Patrol,         //�p�j
    Find,           //����
    Chase,          //�Ǐ]
    Buttle,         //�U��
    LostPatrol,     //����������̃p�g���[���X�e�[�g
    ComebackPatrol  //�p�j���[�g�ɖ߂鏈��
}

public struct AIStator_TransitionMember
{

}

[RequireComponent(typeof(EyeSearchRange))]
public class AIStator : StatorBase<EnemyBase, StateType, TransitionMember>
{
    private EnemyBase m_enemy;
    private EyeSearchRange m_eyeRange;
    private TargetManager m_targetManager;

    protected override void Awake()
    {
        base.Awake();

        m_enemy = GetComponent<EnemyBase>();
        m_eyeRange = GetComponent<EyeSearchRange>();
        m_targetManager = GetComponent<TargetManager>();
    }

    private void Start()
    {
        CreateNode();
        CreateEdge();
    }

    protected override void CreateNode()
    {
        m_stateMachine.AddNode(StateType.None, null);   //None

        m_stateMachine.AddNode(StateType.Patrol, new StateNode.Patrol(m_enemy));    //Patrol

        m_stateMachine.AddNode(StateType.Find, new StateNode.Find(m_enemy));        //Find

        m_stateMachine.AddNode(StateType.Chase, new StateNode.Chase(m_enemy));      //Chase

        m_stateMachine.AddNode(StateType.LostPatrol, new StateNode.LostPatrol(m_enemy));    //LostPatrol

        m_stateMachine.AddNode(StateType.ComebackPatrol, new StateNode.ComebackPatrol(m_enemy)); //ComebackPatrol

        m_stateMachine.AddNode(StateType.Buttle, new StateNode.Buttle(m_enemy));    //Buttle
    }

    protected override void CreateEdge()
    {
        //None
        m_stateMachine.AddEdge(StateType.None, StateType.Patrol, IsGameStartTransition, (int)StateType.Patrol);

        //Patrol
        m_stateMachine.AddEdge(StateType.Patrol, StateType.Chase, IsFindState, (int)StateType.Chase);

        //Chase
        //m_stateMachine.AddEdge(StateType.Chase, StateType.Buttle, IsButtleState, (int)StateType.Buttle);
        m_stateMachine.AddEdge(StateType.Chase, StateType.LostPatrol, IsTrue, (int)StateType.LostPatrol, true);

        //LostPatrol
        m_stateMachine.AddEdge(StateType.LostPatrol, StateType.ComebackPatrol, IsTrue, (int)StateType.ComebackPatrol, true);
        m_stateMachine.AddEdge(StateType.LostPatrol, StateType.Chase, IsFindState, (int)StateType.Chase);

        //ComebackPatrol
        m_stateMachine.AddEdge(StateType.ComebackPatrol, StateType.Patrol, IsTrue, (int)StateType.Patrol, true);
        m_stateMachine.AddEdge(StateType.ComebackPatrol, StateType.Chase, IsFindState, (int)StateType.Chase);

        //Buttle
        m_stateMachine.AddEdge(StateType.Buttle, StateType.Chase, IsTrue, (int)StateType.Chase, true);
    }

    //--------------------------------------------------------------------------------------
    ///	�J�ڏ���
    //--------------------------------------------------------------------------------------

    //��������true�ɂ������ꍇ�B
    bool IsTrue(ref TransitionMember member) { return true; }

    bool IsGameStartTransition(ref TransitionMember member)
    {
        var gameManager = GameManagerComponent.Instance;
        return gameManager.CurrentState == GameManagerComponent.GameState.Game;
    }

    bool IsFindState(ref TransitionMember member)
    {
        var targets = m_enemy.GetObserveIsInEyeTargets().SearchIsInEyeTargets();
        foreach(var target in targets)
        {
            var targeted = target.GetComponent<Targeted>();
            if (targeted && targeted.IsTarget())
            {
                return true;
            }
        }

        return false;
    }

    bool IsButtleState(ref TransitionMember member)
    {
        var target = m_targetManager.GetCurrentTarget();
        if (!target) {
            return false;
        }

        const float AttackRange = 3.0f;
        if(m_eyeRange.IsInEyeRange(target, AttackRange)) {
            return true;
        }

        return false;
    }
}