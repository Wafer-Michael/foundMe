using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ImpactCell : Cell
{
    /// <summary>
    /// �e���f�[�^
    /// </summary>
    public struct ImpactData
    {
        public float dangerValue;       //�댯�l
    }

    private ImpactData m_impactData;    //�e���f�[�^

    public ImpactCell(int index, Parametor parametor) :
        base(index, parametor)
    { }

    public ImpactCell(int index, Parametor parametor, ImpactData impactData) :
        base(index, parametor)
    {
        m_impactData = impactData;
    }

    //--------------------------------------------------------------------------------------
    ///	�A�N�Z�b�T
    //--------------------------------------------------------------------------------------

    public void SetImpactData(ImpactData data) { m_impactData = data; }

    public ImpactData GetImpactData() { return m_impactData; }
}
