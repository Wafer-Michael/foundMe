using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIVirtualController : MonoBehaviour
{
    private Vector2 m_inputDirection;   //入力方向

    /// <summary>
    /// 入力方向の設定
    /// </summary>
    /// <param name="direction">入力方向</param>
    public void SetInputDirection(Vector2 direction) { m_inputDirection = direction; }

    /// <summary>
    /// 入力方向の設定
    /// </summary>
    /// <returns>入力方向</returns>
    public Vector2 GetInputDirection() { return m_inputDirection; }

}
