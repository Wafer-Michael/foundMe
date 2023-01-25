using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PCPlayer : PlayerBase
{

    [SerializeField]
    private Animator m_animator = null;

    private void Start()
    {
        UnityEngine.XR.XRSettings.showDeviceView = false;
    }

    private void Update()
    {
        UpdateAnimation();
    }

    private void UpdateAnimation()
    {
        m_animator?.SetFloat("moveSpeed", PlayerInputer.CalculateMoveVector().magnitude);
    }
}
