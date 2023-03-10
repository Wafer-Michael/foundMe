using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VRPlayer : PlayerBase
{
    [SerializeField]
    //OpenCloseUI m_openCloseUI;
    private VRUI_Ex m_ui;

    private Targeted m_targeted;

    private void Awake()
    {
        m_targeted = GetComponent<Targeted>();

        m_targeted.AddIsTargetEvent(m_ui.IsOpen);  //ターゲット指定の処理(仮実装)
    }
}
