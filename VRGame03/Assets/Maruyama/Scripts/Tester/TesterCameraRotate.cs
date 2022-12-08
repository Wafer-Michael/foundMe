using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TesterCameraRotate : MonoBehaviour
{ 
    [SerializeField]
    private Camera m_camera;        //カメラ

    [SerializeField]
    private float m_speed = 60.0f;          //回転スピード
    public float Speed => m_speed;  //回転スピードのプロパティ

    private PlayerInputer Inputer { get; set; } //入力関係

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
