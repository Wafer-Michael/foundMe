using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEngine.Events;
using UniRx;

public class Item : MonoBehaviour
{
    public enum State
    {
        Idle,           //初期状態
        Access,         //アクセス状態
        Getable,        //取得された状態
    }

    [SerializeField]
    private UnityEvent m_idleEvent = new UnityEvent();      //待機中イベント

    [SerializeField]
    private UnityEvent m_accessEvent = new UnityEvent();    //アクセスイベント

    [SerializeField]
    private UnityEvent m_getableEvent = new UnityEvent();   //ゲットイベント
    
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
