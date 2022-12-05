using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using StateType = AIStator_StateType;
using TransitionMember = AIStator_TransitionMember;

public enum AIStator_StateType 
{
    None,
    Patrol,
    Find,   //”­Œ©
    Buttle, //UŒ‚
}

public struct AIStator_TransitionMember
{

}

[RequireComponent(typeof(EyeSearchRange))]
public class AIStator : StatorBase<EnemyBase, StateType, TransitionMember>
{
    private EnemyBase m_enemy;
    private EyeSearchRange m_eyeRange;

    [SerializeField]
    private List<GameObject> m_observeIsInEyeTargetObjects = new List<GameObject>();

    private ObserveIsInEyeTargets m_observeIsInEyeTargets;  //‹ŠE”ÍˆÍ‚É“ü‚Á‚½‚çƒ^[ƒQƒbƒg”»’è‚ğæ‚éƒ^[ƒQƒbƒg

    protected override void Awake()
    {
        base.Awake();

        m_enemy = GetComponent<EnemyBase>();
        m_eyeRange = GetComponent<EyeSearchRange>();

        m_observeIsInEyeTargets = new ObserveIsInEyeTargets(m_observeIsInEyeTargetObjects, m_eyeRange);
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
    }

    protected override void CreateEdge()
    {
        //None
        m_stateMachine.AddEdge(StateType.None, StateType.Patrol, IsGameStartTransition, (int)StateType.Patrol);

        //Patrol
        m_stateMachine.AddEdge(StateType.Patrol, StateType.Find, IsFindState, (int)StateType.Find);
    }

    //--------------------------------------------------------------------------------------
    ///	‘JˆÚğŒ
    //--------------------------------------------------------------------------------------

    bool IsGameStartTransition(ref TransitionMember member)
    {
        var gameManager = GameManagerComponent.Instance;
        return gameManager.CurrentState == GameManagerComponent.GameState.Game;
    }

    bool IsFindState(ref TransitionMember member)
    {
        var targets = m_observeIsInEyeTargets.SearchIsInEyeTargets();
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
}