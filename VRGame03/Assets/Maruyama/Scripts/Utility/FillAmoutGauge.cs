using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEngine.UI;

public class FillAmoutGauge : MonoBehaviour
{
    [SerializeField]
    private Image m_image;           //�Q�[�W�̃C���[�W�摜

    private float m_fillAmoutValue;  //�t�B���A���[�g�l
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
