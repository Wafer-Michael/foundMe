using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

using OculusSampleFramework;

public class VRUI : MonoBehaviour, I_VRUI
{
    [SerializeField]
    protected UnityEvent m_open;                           //開くイベント群

    [SerializeField]
    protected UnityEvent m_close;                          //閉じるイベント群

    [SerializeField]
    protected UnityEvent<InteractableStateArgs> m_touch;   //タッチイベント群

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
        //タッチしたときのステートなら
        if(obj.NewInteractableState == InteractableState.ActionState)
        {
            m_touch.Invoke(obj);
        }
    }

    //--------------------------------------------------------------------------------------
    /// インスペクタ以外からの登録用
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
