using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OpenDoor : MonoBehaviour
{
    enum State {
        Idle,
        Open,
        OpenIdle,
        Close,
    }

    struct TransitionMember {
        
    }

    State m_state = State.Idle;

    StateMachine<OpenDoor, State, TransitionMember> m_stateMachine = new StateMachine<OpenDoor, State, TransitionMember>();

    private void Awake()
    {
        
    }

    public void Open(GameObject other)
    {
        
    }

    private void Update()
    {
        
    }
}
