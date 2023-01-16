using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// バッテリー型のライト
/// </summary>
public class BatteryLight : MonoBehaviour
{
    [SerializeField]
    private BatteryUser m_batteryUser;  //バッテリーユーザー

    private Light m_light;              //ライト

    private void Awake()
    {
        m_light = GetComponent<Light>();

        if (!m_batteryUser) {
            m_batteryUser = GetComponentInParent<BatteryUser>();
        }
    }

    private void Update()
    {
        m_light.intensity = m_batteryUser.GetBatteryRate();
    }
}
