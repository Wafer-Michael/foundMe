using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// ���ꂼ��̎w�ɓ����B
/// </summary>
public class UITouchMover : MonoBehaviour
{
    private Vector3 m_offset = Vector3.zero;

    /// <summary>
    /// �I�����ɌĂяo������
    /// </summary>
    /// <param name="other"></param>
    public void SelectEvent(GameObject other)
    {
        other.transform.SetParent(transform);
    }

    /// <summary>
    /// �w�����ꂽ���ɌĂяo������
    /// </summary>
    /// <param name="other"></param>
    public void UnSelectEvent(GameObject other)
    {
        other.transform.SetParent(null);
    }
}
