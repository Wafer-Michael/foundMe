using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// �W���b�N������
/// </summary>
public class Jackable : MonoBehaviour
{
    [SerializeField]
    private Camera m_eyeCamera; //���E�J����

    //--------------------------------------------------------------------------------------
    /// �A�N�Z�b�T
    //--------------------------------------------------------------------------------------

    public Camera GetEyeCamera() { return m_eyeCamera; }
}
