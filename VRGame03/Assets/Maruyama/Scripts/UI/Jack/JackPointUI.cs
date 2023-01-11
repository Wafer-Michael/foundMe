using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEngine.Events;

public class JackPointUI : Selectable_VRUI
{
    private Jackable m_jakable = null;      //�����̐ݒ肳��Ă���n�b�L���O����镨

    [SerializeField]
    private UnityEvent<JackPointUI> m_selectEvents;

    public void AddSelectEvent(UnityAction<JackPointUI> action) { m_selectEvents.AddListener(action); }

    public void SelectEventsInvoke() { m_selectEvents.Invoke(this); }

    //--------------------------------------------------------------------------------------
    /// �A�N�Z�b�T
    //--------------------------------------------------------------------------------------

    public void SetJakable(Jackable jakable) { m_jakable = jakable; }

    public Jackable GetJakable() { return m_jakable; }

}
