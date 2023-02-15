using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEngine.Events;
using UniRx;

public class Item : MonoBehaviour
{
    public enum State
    {
        Idle,           //�������
        Access,         //�A�N�Z�X���
        Getable,        //�擾���ꂽ���
    }

    [SerializeField]
    private UnityEvent m_idleEvent = new UnityEvent();

    [SerializeField]
    private UnityEvent m_accessEvent = new UnityEvent();

    [SerializeField]
    private UnityEvent m_getableEvent = new UnityEvent();
    
    private void Awake()
    {
        StateObservable
            .Where(value => value == State.Idle)
            .Subscribe(value => m_idleEvent?.Invoke())
            .AddTo(this);

        StateObservable
            .Where(value => value == State.Access)
            .Subscribe(value => m_accessEvent?.Invoke())
            .AddTo(this);

        StateObservable
            .Where(value => value == State.Getable)
            .Subscribe(value => m_getableEvent?.Invoke())
            .AddTo(this);
    }

    private ReactiveProperty<State> m_state = new ReactiveProperty<State>(State.Idle);
    public State CurrentState => m_state.Value;
    public void SetState(State state) { m_state.Value = state; }
    public System.IObservable<State> StateObservable => m_state;
}
