using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// ジャックされる者
/// </summary>
public class Jackable : MonoBehaviour
{
    [SerializeField]
    private Camera m_eyeCamera;             //視界カメラ

    [SerializeField]
    private RenderTexture m_renderTexture;  //レンダーテクスチャ

    public void UISelectEvent(bool isSelect)
    {
        if (isSelect) {
            m_eyeCamera.targetTexture = m_renderTexture;
        }
        else {
            m_eyeCamera.targetTexture = null;
        }
    }

    //--------------------------------------------------------------------------------------
    /// アクセッサ
    //--------------------------------------------------------------------------------------

    public Camera GetEyeCamera() { return m_eyeCamera; }
}
