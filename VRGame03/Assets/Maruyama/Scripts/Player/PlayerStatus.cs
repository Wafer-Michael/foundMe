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
    private Parametor m_param;

    private DamageEffectController m_damageEffect;
    private PlayerStator m_stator;

    private void Awake()
    {
        m_stator = GetComponent<PlayerStator>();
        m_damageEffect = GetComponent<DamageEffectController>();
    }

    private void Update()
    {
        
    }

    public void Damaged(DamageData data)
    {
        m_param.hp += -data.damageValue;

        if(IsDeath())
        {
            m_param.hp = 0.0f;
            GameManagerComponent.Instance.ChangeState(GameManagerComponent.GameState.GameOver);     //ゲームオーバー処理
            return;
        }

        m_stator.ChangeState(PlayerStator.StateType.Normal);
        m_damageEffect.EffectStart();
        
        //Debug.Log("hp: " + m_param.hp.ToString());
    }

    public bool IsDeath() { return m_param.hp <= 0; }
    
}
