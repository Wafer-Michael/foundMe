using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Blink : MonoBehaviour
{
    [SerializeField]
    float m_speed = 5.0f;

    private Vector3 StartPosition { get; set; }
    private Vector3 TargetPosition { get; set; }
    private Vector3 ToTargetPosition { get { return TargetPosition - StartPosition; } }

    private bool m_isMove = false;
    public bool IsMove => m_isMove;
    private float MoveRange { get; set; }

    //private float MoveElapsedRange { get; set; }
    private float MoveElapsedTime { get; set; }

    void Update()
    {
        if (IsMove)
        {
            MoveUpdate();
        }
    }

    void MoveUpdate()
    {
        MoveElapsedTime += m_speed * Time.deltaTime;
        transform.position = Vector3.Lerp(StartPosition, TargetPosition, MoveElapsedTime);
        //transform.position += ToTargetPosition.normalized * m_speed * Time.deltaTime;

        if(MoveElapsedTime > 1.0f)
        {
            m_isMove = false;
        }

        //MoveElapsedRange = (transform.position - StartPosition).magnitude;

        //if (MoveRange < MoveElapsedRange)
        //{
        //    m_isMove = false;
        //}
    }

    public void StartMove(Vector3 startPosition, Vector3 targetPosition)
    {
        if (IsMove)
        {
            return;
        }

        m_isMove = true;

        StartPosition = startPosition;
        TargetPosition = targetPosition;

        MoveRange = ToTargetPosition.magnitude;
        MoveElapsedTime = 0.0f;
    }
}
