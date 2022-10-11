using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TesterMover : MonoBehaviour
{
    [SerializeField]
    float m_speed = 3.0f;
    public float Speed => m_speed;

    PlayerInputer Inputer { get; set; }

    private void Awake()
    {
        Inputer = GetComponent<PlayerInputer>();
    }

    void Start()
    {
        
    }

    void Update()
    {
        UpdateMove();
        UpdateCameraDirection();
    }

    void UpdateMove()
    {
        var moveVec = Inputer.CalculateMoveVector();

        transform.position += moveVec * Speed * Time.deltaTime;
    }

    void UpdateCameraDirection()
    {
        
    }
}
