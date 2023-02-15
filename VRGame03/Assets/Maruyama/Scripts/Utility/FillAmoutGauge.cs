using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEngine.UI;

public class FillAmoutGauge : MonoBehaviour
{
    [SerializeField]
    private Image m_image;           //ゲージのイメージ画像

    private float m_fillAmoutValue;  //フィルアモート値
    public float FillAmoutValue {
        set => m_fillAmoutValue = value;
        get => m_fillAmoutValue;
    }

    private void Awake()
    {
        if (!m_image) {
            m_image = GetComponent<Image>();
        }
    }

    private void Update()
    {
        m_image.fillAmount = m_fillAmoutValue;
    }
}
