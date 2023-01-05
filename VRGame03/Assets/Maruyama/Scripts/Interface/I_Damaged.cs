using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public struct DamageData
{
    public float damageValue;       //ダメージ値
    public GameObject attacker;     //ダメージを与えて来た相手

    public DamageData(float value, GameObject attacker)
    {
        this.damageValue = value;
        this.attacker = attacker;
    }
}


public interface I_Damaged
{
    /// <summary>
    /// ダメージを受けた時に受ける値
    /// </summary>
    /// <param name="data">ダメージデータ</param>
    public void Damaged(DamageData data);
}
