using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Battery : MonoBehaviour
{
    public static readonly Parametor DEFAULT_PARAMETOR = new Parametor()
    {
        maxValue = 100,
        value = 100
    };

    [System.Serializable]
    public struct Parametor
    {
        public float value;     //���݂̓d�r�c��
        public float maxValue;  //�ő�d�r�c��
    }

    [SerializeField]
    private Parametor m_param = DEFAULT_PARAMETOR;  //�p�����[�^

    public void SetValue(float value) { m_param.value = Mathf.Clamp(value, 0, MaxValue); }

    public float GetValue() { return m_param.value; }

    public float MaxValue => m_param.maxValue;

    public float GetBatteryRate() { return GetValue() / MaxValue; }
}
