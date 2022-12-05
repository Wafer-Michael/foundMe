using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEngine.Events;

public class Targeted : MonoBehaviour
{
    [SerializeField]
    System.Func<bool> m_isTargetEvent = null;

    /// <summary>
    /// �^�[�Q�b�g�w��ł��邩�ǂ���
    /// </summary>
    /// <returns></returns>
    public bool IsTarget()
    {
        if(m_isTargetEvent == null)
        {
            Debug.Log("Null�ł�");
            return true;
        }

        return m_isTargetEvent.Invoke();
    }

    public void AddIsTargetEvent(System.Func<bool> func)
    {
        m_isTargetEvent += func;
    }
}
