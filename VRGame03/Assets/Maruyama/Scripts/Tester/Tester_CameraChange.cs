using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tester_CameraChange : MonoBehaviour
{
    [SerializeField]
    Camera m_mainCamera;

    [SerializeField]
    Camera m_changeCamera;

    private void Update()
    {
        if (PlayerInputer.IsDebugKeyDown(KeyCode.W))
        {
            m_mainCamera.stereoTargetEye = StereoTargetEyeMask.None;
            m_changeCamera.stereoTargetEye = StereoTargetEyeMask.Both;

            //m_mainCamera.SetActive(false);
            //m_changeCamera.SetActive(true);
        }
    }
}
