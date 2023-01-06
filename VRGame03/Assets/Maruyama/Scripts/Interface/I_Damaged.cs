using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public struct DamageData
{
    public float damageValue;       //ダメージ値
    public GameObject attacker;     //ダメージを与えて来た相手
    public Collider otherCollider;  //当たり判定に当たったコライダー

    public DamageData(float value, GameObject attacker):
        this(value, attacker, null)
    { }

    public DamageData(float value, GameObject attacker, Collider otherCollider)
    {
        this.damageValue = value;
        this.attacker = attacker;
        this.otherCollider = otherCollider;
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
