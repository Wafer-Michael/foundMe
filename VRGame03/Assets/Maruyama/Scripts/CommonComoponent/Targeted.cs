using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEngine.Events;

public class Targeted : MonoBehaviour
{
    [SerializeField]
    System.Func<bool> m_isTargetEvent = null;

    /// <summary>
    /// ターゲット指定できるかどうか
    /// </summary>
    /// <returns></returns>
    public bool IsTarget()
    {
        if(m_isTargetEvent == null)
        {
            Debug.Log("Nullです");
            return true;
        }

        return m_isTargetEvent.Invoke();
    }

    public void AddIsTargetEvent(System.Func<bool> func)
    {
        m_isTargetEvent += func;
    }
}
