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
    /// �e���f�[�^
    /// </summary>
    [System.Serializable]
    public struct ImpactData
    {
        public float dangerValue;       //�댯�l

        public ImpactData(float dengerValue)
        {
            this.dangerValue = dengerValue;
        }
    }

    private ImpactData m_impactData;    //�e���f�[�^

    public ImpactCell(int index, Parametor parametor) :
        this(index, parametor, DEFAULT_IMPACTDATA)
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
