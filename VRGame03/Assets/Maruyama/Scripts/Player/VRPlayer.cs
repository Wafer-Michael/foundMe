using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VRPlayer : PlayerBase
{
    [SerializeField]
    OpenCloseUI m_openCloseUI;

    private Targeted m_targeted;

    private void Awake()
    {
        m_targeted = GetComponent<Targeted>();

        m_targeted.AddIsTargetEvent(m_openCloseUI.IsDraw);  //�^�[�Q�b�g�w��̏���(������)
    }
}
