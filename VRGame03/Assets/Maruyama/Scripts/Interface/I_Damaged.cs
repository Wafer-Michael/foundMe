using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public struct DamageData
{
    public float damageValue;       //�_���[�W�l
    public GameObject attacker;     //�_���[�W��^���ė�������

    public DamageData(float value, GameObject attacker)
    {
        this.damageValue = value;
        this.attacker = attacker;
    }
}


public interface I_Damaged
{
    /// <summary>
    /// �_���[�W���󂯂����Ɏ󂯂�l
    /// </summary>
    /// <param name="data">�_���[�W�f�[�^</param>
    public void Damaged(DamageData data);
}
