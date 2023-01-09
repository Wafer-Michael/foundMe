using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEngine.Events;

public class UICloseController : MonoBehaviour
{
    public static readonly Parametor DEFAULT_PARAMETOR = new Parametor()
    {
        speed = 5.0f,
        minSizeRatio = 0.25f
    };

    [System.Serializable]
    public struct Parametor
    {
        public float speed;         //開く速度
        public float minSizeRatio;  //大きさの最大値
    }

    [SerializeField]
    private Parametor m_param = DEFAULT_PARAMETOR;      //パラメータ

    [SerializeField]
    private UIStretchController m_stretchController;    //引き延ばしコントローラー

    [SerializeField]
    private UnityEvent m_endEvents;

    private System.Action m_updateAction = null;        //更新中のイベントデリゲート

    private void Awake()
    {
        if (!m_stretchController) {
            m_stretchController = GetComponentInParent<UIStretchController>();
        }
    }

    private void Update()
    {
        m_updateAction?.Invoke();
    }

    private void UpdateProcess()
    {
        float currentRatio = m_stretchController.GetCurrentRatio();
        float ratio = currentRatio + -m_param.speed * Time.deltaTime;
        m_stretchController.StretchUpdate(ratio);

        if (IsEnd())
        {
            m_stretchController.StretchUpdate(m_param.minSizeRatio);
            m_endEvents?.Invoke();
            m_updateAction = null;
        }
    }

    public void StartClose()
    {
        m_updateAction = UpdateProcess;
    }


    public bool IsEnd()
    {
        return m_stretchController.GetCurrentRatio() <= m_param.minSizeRatio;
    }
    public bool IsUpdate()
    {
        return m_updateAction != null;
    }
}
