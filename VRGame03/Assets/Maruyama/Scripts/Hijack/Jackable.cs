using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// ジャックされる者
/// </summary>
public class Jackable : MonoBehaviour
{
    [SerializeField]
    private Camera m_eyeCamera; //視界カメラ

    //--------------------------------------------------------------------------------------
    /// アクセッサ
    //--------------------------------------------------------------------------------------

    public Camera GetEyeCamera() { return m_eyeCamera; }
}
