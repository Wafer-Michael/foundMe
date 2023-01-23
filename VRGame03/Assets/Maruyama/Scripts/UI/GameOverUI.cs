using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameOverUI : MonoBehaviour
{
    [SerializeField]
    private SpriteRenderer m_spriteRender;

    private void Awake()
    {
        if(m_spriteRender == null) {
            m_spriteRender = GetComponent<SpriteRenderer>();
        }
    }

    private void Update()
    {
        //ゲームオーバー状態になったら
        if(GameManagerComponent.Instance.CurrentState == GameManagerComponent.GameState.GameOver) {
            m_spriteRender.enabled = true;
        }
    }
}
