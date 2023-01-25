using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIVirtualController : MonoBehaviour
{
    private Vector2 m_inputDirection;           //“ü—Í•ûŒü

    /// <summary>
    /// “ü—Í•ûŒü‚Ìİ’è
    /// </summary>
    /// <param name="direction">“ü—Í•ûŒü</param>
    public void SetInputDirection(Vector2 direction) { m_inputDirection = direction; }

    /// <summary>
    /// “ü—Í•ûŒü‚Ìİ’è
    /// </summary>
    /// <returns>“ü—Í•ûŒü</returns>
    public Vector2 GetInputDirection() { return m_inputDirection; }

    

}
