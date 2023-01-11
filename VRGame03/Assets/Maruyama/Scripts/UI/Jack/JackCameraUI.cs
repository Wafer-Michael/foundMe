using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JackCameraUI : MonoBehaviour
{
    [SerializeField]
    private RenderTexture m_renderTexture;

    private Jackable m_currentJakable = null;

    private void Awake()
    {
        
    }

    /// <summary>
    /// �I�����̃C�x���g
    /// </summary>
    public void SelectEvent(Jackable jakable) 
    {
        jakable.GetEyeCamera().targetTexture = m_renderTexture;
        m_currentJakable = jakable;
    }

    /// <summary>
    /// �I���������̃C�x���g
    /// </summary>
    public void UnSelectEvent()
    {
        m_currentJakable.GetEyeCamera().targetTexture = null;
        m_currentJakable = null;
    }
}
