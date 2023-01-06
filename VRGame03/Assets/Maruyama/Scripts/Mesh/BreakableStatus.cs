using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using System.Linq;

public class BreakableStatus : MonoBehaviour
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

    //private GK.BreakableSurface m_breakableSurface; //壊れる処理をまとめた者(子クラスとして作る)

    private void Awake()
    {
        //m_breakableSurface = GetComponentInChildren<GK.BreakableSurface>();
    }

    public void Damaged(DamageData data, GK.BreakableSurface breakableSurface)
    {
        m_param.hp += -data.damageValue;

        if (IsDead()) { //死亡したなら
            m_param.hp = 0.0f;
            DeadProcess();
        }

        //メッシュを割る処理
        breakableSurface?.Break(breakableSurface.transform.InverseTransformPoint(CalculationHitPoint(data)));
        //Break(data);
    }

    private Vector3 CalculationHitPoint(DamageData data)
    {
        //ここでヒットポイントを計算
        var hitPoint = data.otherCollider.ClosestPoint(data.attacker.transform.position);
        return hitPoint;
    }

    /// <summary>
    /// メッシュを割る処理(現在使用していない)
    /// </summary>
    /// <param name="data">ダメージデータ</param>
    private void Break(DamageData data)
    {
        if (!data.otherCollider) {
            Debug.Log("BreakableStatus::Break(DamageData data) : Colliderが存在しないため、メッシュを割れない");
            return;
        }

        var breakableSurface = data.otherCollider.GetComponent<GK.BreakableSurface>();
        if(breakableSurface == null) {
            Debug.Log("BreakableStatus::Break(DamageData data) : BreakableSurfaceが存在しないため、メッシュを割れない");
            return;
        }

        var hitPoint = CalculationHitPoint(data);   //当たった場所の計算
        var children = GetComponentsInChildren<GK.BreakableSurface>();

        //一番近い場所を取得するためにソート
        var sortList = children.OrderBy(value =>
            {
                var childHitPoint = value.Collider.ClosestPoint(hitPoint);
                return (childHitPoint - hitPoint).magnitude;
            }
        );

        //一番近いターゲットでブレイクさせる。
        var target = sortList.ToArray()[0];
        target.Break(target.transform.InverseTransformPoint(hitPoint));
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
