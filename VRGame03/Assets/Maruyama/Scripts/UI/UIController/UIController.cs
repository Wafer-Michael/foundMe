using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIController : MonoBehaviour
{
    [SerializeField]
    VRUI m_ui;
    public void Open()
    {
        m_ui.Open();
    }

    public void Close()
    {
        m_ui.Close();
    }

    public void Touch()
    {
        //m_ui.Touch();
    }
}
