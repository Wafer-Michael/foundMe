using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEngine.Events;

using Oculus.Interaction;

/// <summary>
/// ��苗���X�N���[��������Ăяo�������C�x���g
/// </summary>
public class UIScrollRangeEvent : MonoBehaviour
{
    [SerializeField]
    private float m_range = 0.05f;  //�C�x���g�𔭐������鋗��

    [SerializeField]
    private UnityEvent<PointerEvent> m_successEvents;   //�����𒴂����ꍇ�ɌĂяo�������C�x���g

    [SerializeField]
    private UnityEvent<PointerEvent> m_failureEvents;   //�������z���Ȃ������ꍇ�ɌĂяo�������C�x���g

    private UIScrollController m_scrollController;      //�X�N���[���R���g���[��

    private void Awake()
    {
        m_scrollController = GetComponent<UIScrollController>();
    }

    /// <summary>
    /// UnSelect���ɌĂяo������
    /// </summary>
    /// <param name="pointerEvent"></param>
    public void Touch_UnSelect(PointerEvent pointerEvent)
    {
        if(!m_scrollController) {
            return;
        }

        //��苗���𒴂��Ă�����
        if (IsOverRange()) {
            m_successEvents?.Invoke(pointerEvent);
        }
        else {
            m_failureEvents?.Invoke(pointerEvent);
        }
    }

    /// <summary>
    /// ��苗�������Ă��邩�ǂ���
    /// </summary>
    /// <returns></returns>
    private bool IsOverRange()
    {
        var toCurrentVec = m_scrollController.transform.position - m_scrollController.InitializePosition;
        var currentRange = toCurrentVec.magnitude;

        return currentRange >= m_range;
    }
}
