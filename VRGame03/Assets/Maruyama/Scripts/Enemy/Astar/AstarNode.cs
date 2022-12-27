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

    public AstarNode(int index) :
        this(index, Vector3.zero)
    { }

    public AstarNode(int index, Vector3 position) :
        base(index)
    {
        m_param.position = position;
    }

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

}
