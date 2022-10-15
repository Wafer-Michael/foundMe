using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ColorFilter : MonoBehaviour
{
    private int m_currentIndex = 0;

    private Renderer m_renderer;

    private void Awake()
    {
        m_renderer = GetComponent<Renderer>();
    }

    public void ChangeColor(Color color)
    {
        m_renderer.material.color = color;
    }
}
