using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UniRx;

public class Item : MonoBehaviour
{
    public enum State
    {
        Idle,           //初期状態
        Access,         //アクセス状態
        Getable,        //取得された状態
    } 

    private ReactiveProperty<State> m_state = new ReactiveProperty<State>(State.Idle);
    public State CurrentState => m_state.Value;
    public void SetState(State state) { m_state.Value = state; }
    public System.IObservable<State> StateObservable => m_state;
}
