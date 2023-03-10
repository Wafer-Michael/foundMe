using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UniRx;

public class InputAccessTrigger : MonoBehaviour
{
    public enum State { 
        Idle,
        Access
    }

    [SerializeField]
    private string m_uiName;
    public string GetUIName() => m_uiName;

    [SerializeField]
    private List<GameObject> m_uiTexts = new List<GameObject>();   //テキスト用のUI

    [SerializeField]
    private I_InputAccess m_inputAccess;    //アクセス

    private UniRx.ReactiveProperty<State> m_state = new UniRx.ReactiveProperty<State>(State.Idle);
    public State CurrentState => m_state.Value;
    public System.IObservable<State> ObservableState => m_state;

    private void Awake()
    {
        m_inputAccess = GetComponentInParent<I_InputAccess>();

        //ステートの切り替わり時に呼び出したい処理
        ObservableState
            .Where(value => value == State.Idle)
            .Subscribe(value => ChangeUITests(false))
            .AddTo(this);

        ObservableState
            .Where(value => value == State.Access)
            .Subscribe(value => ChangeUITests(true))
            .AddTo(this);
    }

    private void ChangeState(State state)
    {
        m_state.Value = state;
    }

    private void ChangeUITests(bool isActive)
    {
        foreach(var ui in m_uiTexts)
        {
            ui.SetActive(isActive);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        var inputAccessController = other.GetComponent<InputAccessController>();
        if (!inputAccessController) {
            return;
        }

        inputAccessController.AddInputAccess(m_inputAccess);
        ChangeState(State.Access);
    }

    private void OnTriggerExit(Collider other)
    {
        var inputAccessController = other.GetComponent<InputAccessController>();
        if (!inputAccessController) {
            return;
        }

        inputAccessController.RemoveInputAccess(m_inputAccess);
        ChangeState(State.Idle);
    }

}
