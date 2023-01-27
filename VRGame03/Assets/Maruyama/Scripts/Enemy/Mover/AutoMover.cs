﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using MaruUtility;

[RequireComponent(typeof(VelocityManager))]
[RequireComponent(typeof(RotationController))]
public class AutoMover : MonoBehaviour
{
    enum MoveType
    {
        Velocity,
        Transform,
    }

    [SerializeField]
    MoveType m_moveType = MoveType.Velocity;

    [SerializeField]
    private float m_moveSpeedPerSecond = 1.0f;

    [SerializeField]
    private bool m_isPositionLoop = false;

    [SerializeField]
    private List<Transform> m_transforms;

    private float m_countRange = 0.0f;

    private int m_nowIndex = 0;

    private bool m_isBack = false;

    private VelocityManager m_velocityManager;

    private RotationController m_rotationController;

    bool IsRotation { get; set; } = false;

    private Vector3 m_initializePosition;

    private void Awake()
    {
        m_rotationController = GetComponent<RotationController>();
        m_velocityManager = GetComponent<VelocityManager>();

        m_initializePosition = transform.position;
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (IsNotMove())  {
            return;
        }

        if (IsRotation) {
            RotationUpdate();
            //return;
        }

        System.Action action = m_moveType switch
        {
            MoveType.Transform => TransformMove,
            MoveType.Velocity => VelocityMove,
            _ => null
        };

        action?.Invoke();
    }

    private void TransformMove()
    {
        var position = CalculatePosition();

        var direction = position - transform.position;
        m_rotationController.SetDirection(direction);
        transform.position = position;
    }

    private void VelocityMove()
    {
        var targetPosition = CalculateVelocityNextPosition();

        var toTargetVec = targetPosition - transform.position;
        m_rotationController.SetDirection(toTargetVec);

        var force = maru.CalculateVelocity.SeekVec(m_velocityManager.velocity, toTargetVec, m_moveSpeedPerSecond);
        
        m_velocityManager.AddForce(force);
    }

    private Vector3 CalculateVelocityNextPosition()
    {
        if (m_transforms.Count == 1)
        {
            return m_transforms[0].position;
        }

        int nextIndex = GetNextIndex();

        var nextPosition = m_transforms[nextIndex].position;

        var distance = (nextPosition - transform.position).magnitude;

        var changeRange = m_moveSpeedPerSecond * Time.deltaTime;
        if (distance <= changeRange) {
            m_nowIndex = nextIndex;
        }

        return nextPosition;
    }

    private Vector3 CalculatePosition()
    {
        if(m_transforms.Count == 1)
        {
            return m_transforms[0].position;
        }

        if(IsNotMove())
        {
            return transform.position;
        }


        m_countRange += m_moveSpeedPerSecond * Time.deltaTime;

        int nextIndex = GetNextIndex();

        float length = 0.0f;

        while (true)
        {
            length = (m_transforms[nextIndex].position - m_transforms[m_nowIndex].position).magnitude;

            if(m_countRange < length)
            {
                break;
            }

            //方向が変わったとき。
            //方向転換スタート
            //IsRotation = true;

            m_countRange -= length;

            m_nowIndex = nextIndex;
            nextIndex = GetNextIndex();

            //Debug.Log($"始まり {m_nowIndex},終わり {nextIndex}");
        }

        return Vector3.Lerp(m_transforms[m_nowIndex].position, m_transforms[nextIndex].position, m_countRange / length);
    }

    private int GetNextIndex()
    {
        if (m_isPositionLoop)
        {
            return m_nowIndex < m_transforms.Count - 1 ? m_nowIndex + 1 : 0;
        }
        else
        {
            if(m_isBack)
            {
                if(m_nowIndex != 0)
                {
                    return m_nowIndex - 1;
                }

                m_isBack = false;
                return 1;
            }
            
            if(m_nowIndex < m_transforms.Count - 1)
            {
                return m_nowIndex + 1;
            }

            m_isBack = true;
            return m_nowIndex - 1;
        }
    }

    private bool IsNotMove()
    {
        if (m_transforms.Count == 0)
        {
            return true;
        }

        if(m_transforms[0] == null || m_transforms[m_nowIndex] == null)
        {
            return true;
        }

        bool isSame = true;

        var initialPosition = m_transforms[0].position;

        foreach(var trans in m_transforms)
        {
            if(trans.position != initialPosition)
            {
                isSame = false;
                break;
            }
        }

        return isSame;
    }

    void RotationUpdate()
    {
        var nextIndex = GetNextIndex();
        var direction = (m_transforms[nextIndex].position - m_transforms[m_nowIndex].position);

        if (Vector3.Angle(Camera.main.transform.forward, direction) < 0.25f)
        {
            IsRotation = false;
        }

        m_rotationController.SetDirection(direction);
    }

    public Vector3 GetFirstPosition()
    {
        if(m_transforms.Count == 0) {
            return m_initializePosition;
        }

        return m_transforms[0].position;
    }

    public void ResetProcess() { m_nowIndex = 0; }
}
