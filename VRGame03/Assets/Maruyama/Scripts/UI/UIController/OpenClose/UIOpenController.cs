using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Oculus.Interaction;
using UnityEngine.Events;

public class UIOpenController : MonoBehaviour
{
    public static readonly Parametor DEFAULT_PARAMETOR = new Parametor() {
        speed = 5.0f,
        maxSizeRatio = 1.0f
    };

    [System.Serializable]
    public struct Parametor
    {
        public float speed;         //�J�����x
        public float maxSizeRatio;  //�傫���̍ő�l
    }

    [SerializeField]
    private Parametor m_param = DEFAULT_PARAMETOR;      //�p�����[�^

    [SerializeField]
    private UIStretchController m_stretchController;    //�������΂��R���g���[���[

    [SerializeField]
    private UnityEvent m_endEvents;

    private System.Action m_updateAction = null;        //�X�V���̃C�x���g�f���Q�[�g

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
        float ratio = currentRatio + m_param.speed * Time.deltaTime;
        m_stretchController.StretchUpdate(ratio);

        if (IsEnd()) {
            m_stretchController.StretchUpdate(m_param.maxSizeRatio);
            m_endEvents?.Invoke();
            m_updateAction = null;
        }
    }

    public void StartOpen()
    {
        m_updateAction = UpdateProcess;
    }

    public bool IsEnd()
    {
        return m_stretchController.GetCurrentRatio() >= m_param.maxSizeRatio;
    }

    public bool IsUpdate()
    {
        return m_updateAction != null;
    }

}
