using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BreakableStatus : MonoBehaviour, I_Damaged
{
    public static readonly Parametor DEFAULT_PARAMETOR = new Parametor(3.0f);

    [System.Serializable]
    public struct Parametor
    {
        public float hp;    //何度までダメージを耐えきることができるかどうか

        public Parametor(float hp)
        {
            this.hp = hp;
        }
    }

    [SerializeField]
    private Parametor m_param = DEFAULT_PARAMETOR;  //パラメータ
    public Parametor Param => m_param;

    private GK.BreakableSurface m_breakableSurface; //壊れる処理をまとめた者(子クラスとして作る)

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

        //メッシュを割る処理
        var hitPoint = CalculationHitPoint(data);   //当たった方向を計算
        Break(hitPoint);
    }

    private Vector2 CalculationHitPoint(DamageData data)
    {
        var result = Vector2.zero;

        //ここでヒットポイントを計算

        return result;
    }

    /// <summary>
    /// メッシュを割る処理
    /// </summary>
    /// <param name="position">当たった場所</param>
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
