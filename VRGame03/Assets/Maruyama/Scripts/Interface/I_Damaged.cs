using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public struct DamageData
{
    public float damageValue;   //�_���[�W�l
}


public interface I_Damaged
{
    /// <summary>
    /// �_���[�W���󂯂����Ɏ󂯂�l
    /// </summary>
    /// <param name="data">�_���[�W�f�[�^</param>
    public void Damaged(DamageData data);
}
