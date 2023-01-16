using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PCPlayer : PlayerBase
{
    private void Start()
    {
        UnityEngine.XR.XRSettings.showDeviceView = false;
    }
}
