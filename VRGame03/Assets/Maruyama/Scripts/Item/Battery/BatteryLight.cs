using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// �o�b�e���[�^�̃��C�g
/// </summary>
public class BatteryLight : MonoBehaviour
{
    [SerializeField]
    private BatteryUser m_batteryUser;  //�o�b�e���[���[�U�[

    private Light m_light;              //���C�g

    private float m_maxIntensity;       //�ő�l�̌�

    private void Awake()
    {
        m_light = GetComponent<Light>();

        if (!m_batteryUser) {
            m_batteryUser = GetComponentInParent<BatteryUser>();
        }

        m_maxIntensity = m_light.intensity;
    }

    private void Update()
    {
        m_light.intensity = m_maxIntensity * m_batteryUser.GetBatteryRate();
    }
}
