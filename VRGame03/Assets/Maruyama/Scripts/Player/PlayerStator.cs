using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStator : StatorBase<PlayerBase, PlayerStator.StateType, PlayerStator.TransitionMember>
{
    public enum StateType
    {
        Normal,
        DoorLock,
    }    

    public struct TransitionMember
    {

    }
    private void Start()
    {
        CreateNode();
        CreateEdge();
    }

    protected override void CreateNode()
    {
        //Debug.Log("ÅöCreateNode");
        var player = GetComponent<PlayerBase>();

        m_stateMachine.AddNode(StateType.Normal, new EmptyState(player));

        m_stateMachine.AddNode(StateType.DoorLock, new DoorLockState(player));
    }

    protected override void CreateEdge()
    {

    }

    class EmptyState : EnemyStateNodeBase<PlayerBase>
    {
        public EmptyState(PlayerBase owner) :
            base(owner)
        {}

        public override bool OnUpdate()
        {
            return true;
        }
    }

}
