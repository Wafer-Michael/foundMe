using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tester_DrawMode : MonoBehaviour
{
    private SpriteRenderer m_spriteRender;

    private void Awake()
    {
        m_spriteRender = GetComponent<SpriteRenderer>();
    }

    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.S))
        {
            m_spriteRender.size += Vector2.right;
        }
    }
}
