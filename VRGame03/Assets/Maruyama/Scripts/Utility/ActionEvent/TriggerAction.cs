﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

using System;

public class TriggerAction : MonoBehaviour
{
    #region メンバ変数

    [SerializeField]
    protected UnityEvent<Collider> m_enterAction = null;
    [SerializeField]
    protected UnityEvent<Collider> m_stayAction = null;
    [SerializeField]
    protected UnityEvent<Collider> m_exitAction = null;

    #endregion

    #region Enter

    public void AddEnterAction(UnityAction<Collider> action)
    {
        m_enterAction.AddListener(action);
    }

    public void SetEnterAction(UnityEvent<Collider> action)
    {
        m_enterAction = action;
    }

    #endregion

    #region Stay

    public void AddStayAction(UnityAction<Collider> action)
    {
        m_stayAction.AddListener(action);
    }

    public void SetStayAction(UnityEvent<Collider> action)
    {
        m_stayAction = action;
    }

    #endregion

    #region Exit

    public void AddExitAction(UnityAction<Collider> action)
    {
        m_exitAction.AddListener(action);
    }

    public void SetExitAction(UnityEvent<Collider> action)
    {
        m_exitAction = action;
    }

    #endregion

    #region OnTrigger

    private void OnTriggerEnter(Collider other)
    {
        m_enterAction?.Invoke(other);
    }

    private void OnTriggerStay(Collider other)
    {
        m_stayAction?.Invoke(other);
    }

    private void OnTriggerExit(Collider other)
    {
        m_exitAction?.Invoke(other);
    }

    #endregion

}
