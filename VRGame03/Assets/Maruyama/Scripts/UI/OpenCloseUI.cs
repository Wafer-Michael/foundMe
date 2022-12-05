using OculusSampleFramework;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OpenCloseUI : TouchBottunEventBase
{
    [SerializeField]
    private List<GameObject> m_closeUIs;   //•Â‚¶‚éUI”z—ñ

    [SerializeField]
    private bool m_isDraw = false;
    public bool IsDraw()
    {
        return m_isDraw;
    }

    private void Start()
    {
        SetDraws(IsDraw());
    }

    public override void Touch(InteractableStateArgs obj)
    {
        SetDraws(!IsDraw());
        m_isDraw = !m_isDraw;
    }

    private void SetDraws(bool isDraw)
    {
        foreach (var ui in m_closeUIs)
        {
            ui.SetActive(isDraw);
        }
    }
}
