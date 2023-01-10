using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEngine.Events;
using Oculus.Interaction;


public class UIStretchRangeEvent : MonoBehaviour
{
    public static readonly Parametor DEFAULT_PARAMETOR = new Parametor()
    {
        rangeRatio = 0.35f
    };

    [System.Serializable]
    public struct Parametor {
        public float rangeRatio;    //イベントが発生する割合距離   
    }

    [SerializeField]
    private Parametor m_param = DEFAULT_PARAMETOR;

    [SerializeField]
    private UnityEvent m_sucessEvents;

    [SerializeField]
    private UnityEvent m_failureEvents;

    [SerializeField]
    private UIStretchController m_stretchController;

    private void Awake()
    {
        if (!m_stretchController) {
            m_stretchController = GetComponentInParent<UIStretchController>();
        }
    }

    private void Update()
    {
        
    }

    public void Touch_UnSelect(PointerEvent pointer)
    {
        if (!enabled) {
            return;
        }

        //一定以上伸ばしていたら、イベントを呼び出す。
        if (IsOverRange(pointer)) {
            m_sucessEvents?.Invoke();
        }
        else {
            m_failureEvents?.Invoke();
        }
    }

    private bool IsOverRange(in PointerEvent pointer)
    {
        float ratio = m_stretchController.CalculatePositionRatio_Clamp(pointer);
        return ratio >= m_param.rangeRatio;
    }

    //--------------------------------------------------------------------------------------
    /// アクセッサ
    //--------------------------------------------------------------------------------------

    public float GetRatioRange() { return m_param.rangeRatio; }

}
