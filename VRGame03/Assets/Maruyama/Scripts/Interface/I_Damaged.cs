using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public struct DamageData
{
    public float damageValue;   //ダメージ値
}


public interface I_Damaged
{
    /// <summary>
    /// ダメージを受けた時に受ける値
    /// </summary>
    /// <param name="data">ダメージデータ</param>
    public void Damaged(DamageData data);
}
