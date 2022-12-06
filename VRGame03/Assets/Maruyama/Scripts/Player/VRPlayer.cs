using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VRPlayer : Player
{
    [SerializeField]
    OpenCloseUI m_openCloseUI;

    private Targeted m_targeted;

    private void Awake()
    {
        m_targeted = GetComponent<Targeted>();

        m_targeted.AddIsTargetEvent(m_openCloseUI.IsDraw);  //ターゲット指定の処理(仮実装)
    }
}
