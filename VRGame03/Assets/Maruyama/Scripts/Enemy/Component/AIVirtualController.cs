using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIVirtualController : MonoBehaviour
{
    private Vector2 m_inputDirection;   //���͕���

    /// <summary>
    /// ���͕����̐ݒ�
    /// </summary>
    /// <param name="direction">���͕���</param>
    public void SetInputDirection(Vector2 direction) { m_inputDirection = direction; }

    /// <summary>
    /// ���͕����̐ݒ�
    /// </summary>
    /// <returns>���͕���</returns>
    public Vector2 GetInputDirection() { return m_inputDirection; }

}
