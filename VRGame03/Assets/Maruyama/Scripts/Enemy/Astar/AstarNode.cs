using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AstarNode : GraphNode
{
    [System.Serializable]
    public struct Parametor
    {
        public Vector3 position;

        public Parametor(Vector3 position)
        {
            this.position = position;
        }
    }

    [SerializeField]
    private Parametor m_param;  //パラメータ

    private I_GraphNode m_parent;   //親ノード

    #region コンストラクタ

    public AstarNode(int index) :
        this(index, Vector3.zero)
    { }

    public AstarNode(int index, Vector3 position) :
        base(index)
    {
        m_param.position = position;
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

    //public I_GraphNode GetGraphNode() { return m_parent; }

}
