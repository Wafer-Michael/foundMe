using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEngine.Events;

using Oculus.Interaction;

/// <summary>
/// 一定距離スクロールしたら呼び出したいイベント
/// </summary>
public class UIScrollRangeEvent : MonoBehaviour
{
    [SerializeField]
    private float m_range = 0.05f;  //イベントを発生させる距離

    [SerializeField]
    private UnityEvent<PointerEvent> m_successEvents;   //距離を超えた場合に呼び出したいイベント

    [SerializeField]
    private UnityEvent<PointerEvent> m_failureEvents;   //距離を越えなかった場合に呼び出したいイベント

    private UIScrollController m_scrollController;      //スクロールコントローラ

    private void Awake()
    {
        m_scrollController = GetComponent<UIScrollController>();
    }

    /// <summary>
    /// UnSelect時に呼び出したい
    /// </summary>
    /// <param name="pointerEvent"></param>
    public void Touch_UnSelect(PointerEvent pointerEvent)
    {
        if(!m_scrollController) {
            return;
        }

        //一定距離を超えていたら
        if (IsOverRange()) {
            m_successEvents?.Invoke(pointerEvent);
        }
        else {
            m_failureEvents?.Invoke(pointerEvent);
        }
    }

    /// <summary>
    /// 一定距離動いているかどうか
    /// </summary>
    /// <returns></returns>
    private bool IsOverRange()
    {
        var toCurrentVec = m_scrollController.transform.position - m_scrollController.InitializePosition;
        var currentRange = toCurrentVec.magnitude;

        return currentRange >= m_range;
    }

    //--------------------------------------------------------------------------------------
    /// アクセッサ
    //--------------------------------------------------------------------------------------

    public float GetRange() { return m_range; }

}
