using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ImpactCell : Cell
{
    /// <summary>
    /// 影響データ
    /// </summary>
    [System.Serializable]
    public struct ImpactData
    {
        public float dangerValue;       //危険値
    }

    private ImpactData m_impactData;    //影響データ

    public ImpactCell(int index, Parametor parametor) :
        base(index, parametor)
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
}
