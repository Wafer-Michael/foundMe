using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FPSController : MonoBehaviour
{
    [SerializeField]
    private float m_speed;

    private void Update()
    {
        var input = GetInput();

        transform.Rotate(input * m_speed * Time.deltaTime);
    }

    private Vector3 GetInput()
    {
        return PlayerInputer.CalculateMouseCameraMoveVec();
    }
}
