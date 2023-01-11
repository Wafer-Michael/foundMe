using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEngine.Events;

public class JackPointUI : Selectable_VRUI
{
    private Jackable m_jakable = null;      //自分の設定されているハッキングされる物

    [SerializeField]
    private UnityEvent<JackPointUI> m_selectEvents;

    public void AddSelectEvent(UnityAction<JackPointUI> action) { m_selectEvents.AddListener(action); }

    public void SelectEventsInvoke() { m_selectEvents.Invoke(this); }

    //--------------------------------------------------------------------------------------
    /// アクセッサ
    //--------------------------------------------------------------------------------------

    public void SetJakable(Jackable jakable) { m_jakable = jakable; }

    public Jackable GetJakable() { return m_jakable; }

}
