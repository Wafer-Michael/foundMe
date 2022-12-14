using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface I_InputAccess
{
    /// <summary>
    /// 入力からのアクセス
    /// </summary>
    /// <param name="other">アクセス者</param>
    public void Access(GameObject other);
}
