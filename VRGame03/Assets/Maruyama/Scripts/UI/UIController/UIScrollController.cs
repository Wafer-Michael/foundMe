using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Oculus.Interaction;

public class UIScrollController : MonoBehaviour
{
    [SerializeField]
    private float m_maxRange = 5.0f;

    private PointerEvent m_selectPoint;    //選択時のポイント

    public void Touch_Select(PointerEvent pointerEvent)
    {
        m_selectPoint = pointerEvent;
    }

    public void Touch_Mover(PointerEvent pointerEvent)
    {
        var toPoint = pointerEvent.Pose.position - m_selectPoint.Pose.position;

        transform.position = pointerEvent.Pose.position;
    }

    public void Touch_UnSelect(PointerEvent pointerEvent)
    {

    }
}
