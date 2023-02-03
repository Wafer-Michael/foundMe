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

    [SerializeField]
    private GameObject m_uiPrefab;
    private GameObject m_ui;

    [SerializeField]
    private Vector3 m_offset = new Vector3(0.0f, 0.25f, 0.0f);

    private void Start()
    {
        if (m_uiPrefab) {
            m_ui = Instantiate(m_uiPrefab, transform.position + m_offset, Quaternion.identity, transform);
        }
    }

    public void SetValue(float value) { m_param.value = Mathf.Clamp(value, 0, MaxValue); }

    public float GetValue() { return m_param.value; }

    public float MaxValue => m_param.maxValue;

    public float GetBatteryRate() { return GetValue() / MaxValue; }

    public void ChangeUIActive(bool isActive)
    {
        if (m_ui) {
            m_ui.SetActive(isActive);
        }
    }
}
