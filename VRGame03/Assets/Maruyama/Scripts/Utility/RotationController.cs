﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 回転をコントロールする
/// </summary>
public class RotationController : MonoBehaviour
{
    [SerializeField]
    private GameObject m_rotationTarget;

    [SerializeField]
    private float m_rotationSpeed = 3.0f;

    private Vector3 m_direct = new Vector3();

    private void Start()
    {
        if (!m_rotationTarget)
        {
            m_rotationTarget = gameObject;
        }
    }

    private void Update()
    {
        var direct = m_direct;
        direct.y = 0;

        if(direct != Vector3.zero)
        {
            Camera.main.transform.rotation = Quaternion.Lerp(Camera.main.transform.rotation,
                                     Quaternion.LookRotation(direct),
                                     m_rotationSpeed * Time.deltaTime);
        }
    }

    #region アクセッサ

    public void SetDirect(Vector3 direct)
    {
        m_direct = direct;
    }
    public Vector3 GetDirect()
    {
        return m_direct;
    }

    public void SetSpeed(float speed)
    {
        m_rotationSpeed = speed;
    }
    public float GetSpeed()
    {
        return m_rotationSpeed;
    }

    public bool IsRotation
    {
        get
        {
            const float nanRange = 0.1f; //誤差と判断する距離
            var subDirect = m_direct - transform.forward;

            if(subDirect.magnitude <= nanRange)
            {
                return false;
            }
            
            return true;
        }
    }

    #endregion

}
