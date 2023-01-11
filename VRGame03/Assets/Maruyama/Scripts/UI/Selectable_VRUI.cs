using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UniRx;
using Oculus.Interaction;

using UnityEngine.Events;

/// <summary>
/// タッチしたときに選択状態が切り替わるUI
/// </summary>
public class Selectable_VRUI : VRUI_Ex
{
    private UniRx.ReactiveProperty<bool> m_isSelect = new ReactiveProperty<bool>(false);
    public System.IObservable<bool> ObservableIsSelect => m_isSelect;

    [SerializeField]
    private UnityEvent<Selectable_VRUI> m_selectEvents;

    public void AddSelectEvent(UnityAction<Selectable_VRUI> action) { m_selectEvents.AddListener(action); }

    public void SelectEventsInvoke() { m_selectEvents.Invoke(this); }

    public void Touch_Select() { m_isSelect.Value = !m_isSelect.Value; }

    public void SetIsSelect(bool isSelect) { m_isSelect.Value = isSelect; }

    public bool IsSelect() { return m_isSelect.Value; }
}
