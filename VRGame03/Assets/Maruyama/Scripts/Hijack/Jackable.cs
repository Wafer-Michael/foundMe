using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// �W���b�N������
/// </summary>
public class Jackable : MonoBehaviour
{
    [SerializeField]
    private Camera m_eyeCamera;             //���E�J����

    [SerializeField]
    private RenderTexture m_renderTexture;  //�����_�[�e�N�X�`��

    [SerializeField]
    private Vector3 m_positionOffset = new Vector3(0.0f, 0.0f, 0.0f);
    public Vector3 PositionOffset => m_positionOffset;

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
    /// �A�N�Z�b�T
    //--------------------------------------------------------------------------------------

    public Camera GetEyeCamera() { return m_eyeCamera; }
}
