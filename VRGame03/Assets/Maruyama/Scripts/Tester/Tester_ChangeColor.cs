using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tester_ChangeColor : MonoBehaviour
{
    [SerializeField]
    private List<Color> m_colors = new List<Color>();
    public List<Color> Colors => m_colors;

    private int m_currentIndex = 0;

    private Renderer m_renderer;

    private void Awake()
    {
        m_renderer = GetComponent<Renderer>();
    }

    private void Start()
    {
        if (CanChangeColor())
        {
            m_renderer.material.color = Colors[0];
        }
    }

    private void Update()
    {
        if (!CanChangeColor())
        {
            return;
        }

        if (PlayerInputer.IsChangeColor())
        {
            ChangeColor();
        }
    }

    void ChangeColor()
    {
        ChangeCurrentIndex();
        m_renderer.material.color = Colors[m_currentIndex];
    }

    void ChangeCurrentIndex()
    {
        m_currentIndex++;

        if (m_currentIndex >= Colors.Count)
        {
            m_currentIndex = 0;
        }
    }

    bool CanChangeColor()
    {
        return Colors.Count != 0;
    }
}
