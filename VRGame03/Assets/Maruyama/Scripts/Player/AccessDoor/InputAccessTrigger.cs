using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
    private I_InputAccess m_inputAccess;

    private UniRx.ReactiveProperty<State> m_state = new UniRx.ReactiveProperty<State>(State.Idle);
    public State CurrentState => m_state.Value;
    public System.IObservable<State> ObservableState => m_state;

    private void Awake()
    {
        m_inputAccess = GetComponentInParent<I_InputAccess>();
    }

    private void ChangeState(State state)
    {
        m_state.Value = state;
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
