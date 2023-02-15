using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClearUI : MonoBehaviour
{
    //TMPro.TextMeshProUGUI m_debugText;
    private SpriteRenderer m_spriteRender;

    private void Awake()
    {
        m_spriteRender = GetComponent<SpriteRenderer>();
    }

    public void ClearEvent()
    {
        if (!m_spriteRender) {
            return;
        }

        m_spriteRender.enabled = true;
    }
}
