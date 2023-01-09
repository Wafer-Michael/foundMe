using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UniRx;

public class VRUI_Ex : MonoBehaviour
{
    private ReactiveProperty<bool> m_isOpen = new ReactiveProperty<bool>(false);
    public System.IObservable<bool> IsOpenObservable => m_isOpen;

    //--------------------------------------------------------------------------------------
    /// アクセッサ
    //--------------------------------------------------------------------------------------

    public void SetIsOpen(bool isOpen) { m_isOpen.Value = isOpen; }

    public bool IsOpen() { return m_isOpen.Value; }
}
