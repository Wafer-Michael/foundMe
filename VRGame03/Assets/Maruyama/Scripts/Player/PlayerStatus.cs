using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStatus : MonoBehaviour, I_Damaged
{
    [System.Serializable]
    public struct Parametor {
        public float hp;
    }

    [SerializeField]
    Parametor m_param;

    public void Damaged(DamageData data)
    {
        m_param.hp += -data.damageValue;

        if(IsDeath())
        {
            m_param.hp = 0.0f;
            //ゲームオーバー処理
        }

        Debug.Log("hp: " + m_param.hp.ToString());
    }

    public bool IsDeath() { return m_param.hp <= 0; }
    
}
