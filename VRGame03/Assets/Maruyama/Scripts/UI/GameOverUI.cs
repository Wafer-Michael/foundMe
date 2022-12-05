using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameOverUI : MonoBehaviour
{
    TMPro.TextMeshProUGUI m_debugText;

    private void Awake()
    {
        m_debugText = GetComponent<TMPro.TextMeshProUGUI>();
    }

    private void Update()
    {
        if(GameManagerComponent.Instance.CurrentState == GameManagerComponent.GameState.GameOver)
        {
            m_debugText.enabled = true;
        }
    }
}
