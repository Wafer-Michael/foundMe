using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UniRx;

public class Item : MonoBehaviour
{
    public enum State
    {
        Idle,           //�������
        Access,         //�A�N�Z�X���
        Getable,        //�擾���ꂽ���
    } 

    private ReactiveProperty<State> m_state = new ReactiveProperty<State>(State.Idle);
    public State CurrentState => m_state.Value;
    public void SetState(State state) { m_state.Value = state; }
    public System.IObservable<State> StateObservable => m_state;
}
