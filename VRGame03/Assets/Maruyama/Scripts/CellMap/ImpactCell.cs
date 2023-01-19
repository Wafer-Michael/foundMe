using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ImpactCell : Cell
{
    public static readonly ImpactData DEFAULT_IMPACTDATA = new ImpactData()
    {
        dangerValue = 0.5f
    };

    /// <summary>
    /// 影響データ
    /// </summary>
    [System.Serializable]
    public struct ImpactData
    {
        public float dangerValue;       //危険値

        public ImpactData(float dengerValue)
        {
            this.dangerValue = dengerValue;
        }
    }

    private ImpactData m_impactData;    //影響データ

    public ImpactCell(int index, Parametor parametor) :
        this(index, parametor, DEFAULT_IMPACTDATA)
    { }

    public ImpactCell(int index, Parametor parametor, ImpactData impactData) :
        base(index, parametor)
    {
        m_impactData = impactData;
    }

    //--------------------------------------------------------------------------------------
    ///	アクセッサ
    //--------------------------------------------------------------------------------------

    public void SetImpactData(ImpactData data) { m_impactData = data; }

    public ImpactData GetImpactData() { return m_impactData; }

    public void SetDangerValue(float value) { m_impactData.dangerValue = value; }

    public float GetDangerValue() { return m_impactData.dangerValue; }
}
