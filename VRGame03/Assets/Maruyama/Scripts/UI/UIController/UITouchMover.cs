using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// それぞれの指に入れる。
/// </summary>
public class UITouchMover : MonoBehaviour
{
    private Vector3 m_offset = Vector3.zero;

    /// <summary>
    /// 選択時に呼び出す処理
    /// </summary>
    /// <param name="other"></param>
    public void SelectEvent(GameObject other)
    {
        other.transform.SetParent(transform);
    }

    /// <summary>
    /// 指が離れた時に呼び出す処理
    /// </summary>
    /// <param name="other"></param>
    public void UnSelectEvent(GameObject other)
    {
        other.transform.SetParent(null);
    }
}
