using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamageEffectController : MonoBehaviour
{
    [SerializeField]
    private SpriteRenderer m_spriteRenderer;

    private Material m_material;
    private int m_heightPropertyID;

    [SerializeField]
    private float m_maxValue = 0.25f;

    [SerializeField]
    private float m_drawTime = 1.0f; //•\Ž¦ŽžŠÔ

    private GameTimer m_timer = new GameTimer(0);

    private void Awake()
    {
        if (m_spriteRenderer == null)
        {
            m_spriteRenderer = GetComponent<SpriteRenderer>();
        }

        m_heightPropertyID = Shader.PropertyToID("_Height");
        m_material = m_spriteRenderer.material;
    }

    private void Update()
    {
        if (m_timer.IsTimeUp) {
            return;
        }

        float value = m_maxValue * m_timer.IntervalTimeRate;
        m_material.SetFloat(m_heightPropertyID, value);

        m_timer.UpdateTimer();
    }

    public void EffectStart()
    {
        m_material.SetFloat(m_heightPropertyID, m_maxValue);
        m_timer.ResetTimer(m_drawTime);
    }
}
