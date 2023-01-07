using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Oculus.Interaction;

public class UIScrollController : MonoBehaviour
{
    [SerializeField]
    private Vector2 m_maxScope = new Vector2(+0.1f, +0.1f); //動ける最大距離

    [SerializeField]
    private Vector2 m_minScope = new Vector2(-0.1f, -0.1f); //動ける最小距離

    private GameObject m_initializeObject;

    private bool m_isTouch = false;

    private void Awake()
    {
        m_initializeObject = new GameObject("TouchUIInitialize");
        m_initializeObject.transform.position = transform.position;
        m_initializeObject.transform.rotation = transform.rotation;
    }

    /// <summary>
    /// UnityEentWrapに登録する
    /// </summary>
    /// <param name="pointerEvent"></param>
    public void Touch_Select(PointerEvent pointerEvent)
    {
        m_isTouch = true;
    }

    /// <summary>
    /// UnityEvetWrapに登録する
    /// </summary>
    /// <param name="pointerEvent"></param>
    public void Touch_Mover(PointerEvent pointerEvent)
    {
        if (!m_isTouch) {   //タッチ中でないなら
            return;
        }

        transform.position = CalculatePosition(pointerEvent);
    }

    /// <summary>
    /// UnityEventWrapに登録する
    /// </summary>
    /// <param name="pointerEvent"></param>
    public void Touch_UnSelect(PointerEvent pointerEvent)
    {
        m_isTouch = false;
    }

    /// <summary>
    /// 位置の計算
    /// </summary>
    /// <param name="pointerEvent">ポインターイベント</param>
    /// <returns>計算した位置</returns>
    public Vector3 CalculatePosition(PointerEvent pointerEvent)
    {
        var toPoint = m_initializeObject.transform.InverseTransformPoint(pointerEvent.Pose.position);

        //それぞれのクランプ
        toPoint.x = Mathf.Clamp(toPoint.x, m_minScope.x, m_maxScope.x);
        toPoint.y = Mathf.Clamp(toPoint.y, m_minScope.y, m_maxScope.y);

        return m_initializeObject.transform.position + (transform.rotation * toPoint);
    }

}
