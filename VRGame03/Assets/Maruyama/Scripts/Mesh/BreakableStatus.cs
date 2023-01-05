using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BreakableStatus : MonoBehaviour, I_Damaged
{
    public static readonly Parametor DEFAULT_PARAMETOR = new Parametor(3.0f);

    [System.Serializable]
    public struct Parametor
    {
        public float hp;    //���x�܂Ń_���[�W��ς����邱�Ƃ��ł��邩�ǂ���

        public Parametor(float hp)
        {
            this.hp = hp;
        }
    }

    [SerializeField]
    private Parametor m_param = DEFAULT_PARAMETOR;  //�p�����[�^
    public Parametor Param => m_param;

    private GK.BreakableSurface m_breakableSurface; //���鏈�����܂Ƃ߂���(�q�N���X�Ƃ��č��)

    private void Awake()
    {
        m_breakableSurface = GetComponentInChildren<GK.BreakableSurface>();
    }

    public void Damaged(DamageData data)
    {
        m_param.hp += -data.damageValue;

        if (IsDead()) {
            m_param.hp = 0.0f;
            DeadProcess();
        }

        //���b�V�������鏈��
        var hitPoint = CalculationHitPoint(data);   //���������������v�Z
        Break(hitPoint);
    }

    private Vector2 CalculationHitPoint(DamageData data)
    {
        var result = Vector2.zero;

        //�����Ńq�b�g�|�C���g���v�Z

        return result;
    }

    /// <summary>
    /// ���b�V�������鏈��
    /// </summary>
    /// <param name="position">���������ꏊ</param>
    private void Break(Vector2 position)
    {
        m_breakableSurface.Break(position);
    }

    private void DeadProcess()
    {
        var children = GetComponentsInChildren<GK.BreakableSurface>();
        foreach(var child in children)
        {
            child.BreakSplit();
        }
    }

    public bool IsDead()
    {
        return m_param.hp <= 0.0f;
    }
}
