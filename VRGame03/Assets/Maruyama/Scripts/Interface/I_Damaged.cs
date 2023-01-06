using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public struct DamageData
{
    public float damageValue;       //�_���[�W�l
    public GameObject attacker;     //�_���[�W��^���ė�������
    public Collider otherCollider;  //�����蔻��ɓ��������R���C�_�[

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
    /// �_���[�W���󂯂����Ɏ󂯂�l
    /// </summary>
    /// <param name="data">�_���[�W�f�[�^</param>
    public void Damaged(DamageData data);
}
