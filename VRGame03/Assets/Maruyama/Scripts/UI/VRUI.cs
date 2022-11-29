using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

using OculusSampleFramework;

public class VRUI : MonoBehaviour, I_VRUI
{
    [SerializeField]
    protected UnityEvent m_open;                           //�J���C�x���g�Q

    [SerializeField]
    protected UnityEvent m_close;                          //����C�x���g�Q

    [SerializeField]
    protected UnityEvent<InteractableStateArgs> m_touch;   //�^�b�`�C�x���g�Q

    public void Open()
    {
        m_open.Invoke();
    }

    public void Close()
    {
        m_close.Invoke();
    }

    public void Touch(InteractableStateArgs obj)
    {
        //�^�b�`�����Ƃ��̃X�e�[�g�Ȃ�
        if(obj.NewInteractableState == InteractableState.ActionState)
        {
            m_touch.Invoke(obj);
        }
    }

    //--------------------------------------------------------------------------------------
    /// �C���X�y�N�^�ȊO����̓o�^�p
    //--------------------------------------------------------------------------------------

    public void AddOpenEvent(UnityAction open)
    {
        m_open.AddListener(open);
    }

    public void AddCloseEvent(UnityAction close)
    {
        m_close.AddListener(close);
    }

    public void AddToucnEvent(UnityAction<InteractableStateArgs> touch)
    {
        m_touch.AddListener(touch);
    }
}
