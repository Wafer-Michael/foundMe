using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using StateType = AIStator_StateType;
using TransitionMember = AIStator_TransitionMember;

public enum AIStator_StateType 
{
    None,
    Patrol,
}

public struct AIStator_TransitionMember
{

}

public class AIStator : StatorBase<EnemyBase, StateType, TransitionMember>
{
    EnemyBase m_enemy;

    protected override void Awake()
    {
        base.Awake();

        m_enemy = GetComponent<EnemyBase>();
    }

    private void Start()
    {
        CreateNode();
        CreateEdge();
    }

    protected override void CreateNode()
    {
        m_stateMachine.AddNode(StateType.None, null);

        m_stateMachine.AddNode(StateType.Patrol, new StateNode.Patrol(m_enemy));
    }

    protected override void CreateEdge()
    {

    }
}
