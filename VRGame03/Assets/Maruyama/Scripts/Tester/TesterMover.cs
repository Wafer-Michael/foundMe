using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TesterMover : MonoBehaviour
{
    [SerializeField]
    float m_speed = 3.0f;
    public float Speed => m_speed;

    [SerializeField]
    Camera m_camera;

    private void Awake()
    {

    }

    void Start()
    {
        if(m_camera == null)
        {
            m_camera = Camera.main;
        }
    }

    void Update()
    {
        UpdateMove();
    }

    void UpdateMove()
    {
        var moveVec = PlayerInputer.CalculateMoveVector();
        var cnvartVec = maru.Utility.ConvartCameraVec(moveVec, m_camera, gameObject);

        transform.position += cnvartVec * Speed * Time.deltaTime;
    }
}
