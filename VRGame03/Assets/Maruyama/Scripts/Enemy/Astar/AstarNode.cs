using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AstarNode : GraphNode
{
    public struct ImpactData
    {
        public float dangerValue;   //脅威度

        public ImpactData(float dangerValue)
        {
            this.dangerValue = dangerValue;
        }
    }

    [System.Serializable]
    public struct Parametor
    {
        public Vector3 position;        //位置
        public ImpactData impactData;   //影響データ

        public Parametor(Vector3 position) :
            this(position, new ImpactData() { dangerValue = 0.5f })
        { }

        public Parametor(Vector3 position, ImpactData impactData)
        {
            this.position = position;
            this.impactData = impactData;
        }
    }

    [SerializeField]
    private Parametor m_param;      //パラメータ

    private I_GraphNode m_parent;   //親ノード

    #region コンストラクタ

    public AstarNode(int index) :
        this(index, Vector3.zero)
    { }

    public AstarNode(int index, Vector3 position) :
        this(index, new Parametor(position))
    { }

    public AstarNode(int index, Vector3 position, ImpactData impactData):
        this(index, new Parametor(position, impactData))
    { }

    public AstarNode(int index, Parametor parametor) :
        base(index)
    {
        m_param = parametor;
    }

    #endregion

    //--------------------------------------------------------------------------------------
    /// アクセッサ
    //--------------------------------------------------------------------------------------

    public void SetParametor(Parametor parametor) { m_param = parametor; }

    public Parametor GetParametor() => m_param;

    public Vector3 Position
    {
        set => m_param.position = value;
        get => m_param.position;
    }

    public void SetPosition(Vector3 position) { m_param.position = position; }
    public Vector3 GetPosition() => m_param.position;

    public void SetParent(I_GraphNode node) { m_parent = node; }
    public I_GraphNode GetParent() { return m_parent; }

    public void SetImpactData(ImpactData data) { m_param.impactData = data; }

    public ImpactData GetImpactData() { return m_param.impactData; }

    public void SetDangerValue(float value) { m_param.impactData.dangerValue = value; }

    public float GetDangerValue() { return m_param.impactData.dangerValue; }

}
