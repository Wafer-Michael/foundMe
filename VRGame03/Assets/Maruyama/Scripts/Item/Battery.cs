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
        public float value;     //現在の電池残量
        public float maxValue;  //最大電池残量
    }

    [SerializeField]
    private Parametor m_param = DEFAULT_PARAMETOR;  //パラメータ

    public void SetValue(float value) { m_param.value = Mathf.Clamp(value, 0, MaxValue); }

    public float GetValue() { return m_param.value; }

    public float MaxValue => m_param.maxValue;

    public float GetBatteryRate() { return GetValue() / MaxValue; }
}
