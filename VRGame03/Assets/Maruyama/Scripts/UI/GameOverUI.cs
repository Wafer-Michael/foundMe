using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameOverUI : MonoBehaviour
{
    private TMPro.TextMeshProUGUI m_debugText;

    [SerializeField]
    private SpriteRenderer m_spriteRender;

    private void Awake()
    {
        m_debugText = GetComponent<TMPro.TextMeshProUGUI>();

        if(m_spriteRender == null) {
            m_spriteRender = GetComponent<SpriteRenderer>();
        }
    }

    private void Update()
    {
        if(GameManagerComponent.Instance.CurrentState == GameManagerComponent.GameState.GameOver)
        {
            //m_debugText.enabled = true;
            m_spriteRender.enabled = true;
        }
    }
}
