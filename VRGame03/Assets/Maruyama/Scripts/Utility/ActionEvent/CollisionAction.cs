using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class CollisionAction : MonoBehaviour
{
    #region メンバ変数

    [SerializeField]
    protected UnityEvent<Collision> m_enterAction = null;
    [SerializeField]
    protected UnityEvent<Collision> m_stayAction = null;
    [SerializeField]
    protected UnityEvent<Collision> m_exitAction = null;

    #endregion

    #region Enter

    public void AddEnterAction(UnityAction<Collision> action)
    {
        m_enterAction.AddListener(action);
    }

    public void SetEnterAction(UnityEvent<Collision> action)
    {
        m_enterAction = action;
    }

    #endregion

    #region Stay

    public void AddStayAction(UnityAction<Collision> action)
    {
        m_stayAction.AddListener(action);
    }

    public void SetStayAction(UnityEvent<Collision> action)
    {
        m_stayAction = action;
    }

    #endregion

    #region Exit

    public void AddExitAction(UnityAction<Collision> action)
    {
        m_exitAction.AddListener(action);
    }

    public void SetExitAction(UnityEvent<Collision> action)
    {
        m_exitAction = action;
    }

    #endregion

    #region OnCollision

    private void OnCollisionEnter(Collision collision)
    {
        m_enterAction?.Invoke(collision);
    }

    private void OnCollisionStay(Collision collision)
    {
        m_stayAction?.Invoke(collision);
    }

    private void OnCollisionExit(Collision collision)
    {
        m_exitAction?.Invoke(collision);
    }

    #endregion
}
