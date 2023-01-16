using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UpdateNoiseShader : MonoBehaviour
{
    [System.Serializable]
    public struct Parametor
    {
        
    }

    private Renderer m_render;

    private void Awake()
    {
        m_render = GetComponent<Renderer>();
    }

    private void Update()
    {
        
    }
}
