using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JackCameraUI : MonoBehaviour
{
    [SerializeField]
    private RenderTexture m_renderTexture;

    private Jackable m_currentJackable = null;

    /// <summary>
    /// 選択時のイベント
    /// </summary>
    public void SelectEvent(Jackable jackable) 
    {
        jackable.GetEyeCamera().targetTexture = m_renderTexture;
        m_currentJackable = jackable;

        Debug.Log("★: Select");
    }

    /// <summary>
    /// 選択解除時のイベント
    /// </summary>
    public void UnSelectEvent(Jackable jackable)
    {
        m_currentJackable.GetEyeCamera().targetTexture = null;

        if (m_currentJackable == jackable) {
            m_currentJackable = null;
        }

        Debug.Log("★: UnSelect");
    }
}
