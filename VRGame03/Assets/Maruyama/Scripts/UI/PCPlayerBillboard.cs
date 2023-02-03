using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PCPlayerBillboard : MonoBehaviour
{
    private Camera m_camera;

    private void Start()
    {
        var player = FindObjectOfType<PCPlayer>();
        m_camera = player.GetComponentInChildren<Camera>();
    }

    private void Update()
    {
        if (!m_camera) {
            return;
        }

        var toCamera = m_camera.transform.position - transform.position;
        transform.forward = -toCamera;
    }
}
