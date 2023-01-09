using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UniRx;
using UnityEngine.Events;

public class VRUIObserver : MonoBehaviour
{
    [SerializeField]
    private VRUI_Ex m_vrUI;

    [SerializeField]
    private UnityEvent m_openEvents;

    [SerializeField]
    private UnityEvent m_closeEvents;

    private void Awake()
    {
        //nullCheck
        if (!m_vrUI) {
            m_vrUI = GetComponentInParent<VRUI_Ex>();
        }

        //Open状態になったときのイベント群
        m_vrUI.IsOpenObservable.
           Where(value => value).
           Subscribe(value => m_openEvents?.Invoke()).
           AddTo(this);

        //Close状態になったときのイベント群
        m_vrUI.IsOpenObservable
            .Where(value => !value)
            .Subscribe(value => m_closeEvents?.Invoke())
            .AddTo(this);
    }

}
