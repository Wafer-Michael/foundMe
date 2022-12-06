using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TesterCameraRotate : MonoBehaviour
{ 
    [SerializeField]
    private Camera m_camera;        //�J����

    [SerializeField]
    private float m_speed = 60.0f;          //��]�X�s�[�h
    public float Speed => m_speed;  //��]�X�s�[�h�̃v���p�e�B

    private PlayerInputer Inputer { get; set; } //���͊֌W

    private void Awake()
    {
        Inputer = GetComponent<PlayerInputer>();
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
        UpdateCameraRotate();
    }

    void UpdateCameraRotate()
    {
        var moveVec = PlayerInputer.CalculateMouseCameraMoveVec();

        m_camera.transform.Rotate(moveVec * Speed * Time.deltaTime);
    }
}
