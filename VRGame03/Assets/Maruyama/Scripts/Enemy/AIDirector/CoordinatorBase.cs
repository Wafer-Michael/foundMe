using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//--------------------------------------------------------------------------------------
/// コーディネータの基底クラス
//--------------------------------------------------------------------------------------
public abstract class CoordinatorBase
{
    protected List<I_FactionMember> m_members;  //ファクションに所属するメンバー

    private TupleSpace m_tupleSpace;            //タプルスペース

    private CoordinatorBase m_parent;           //親コーディネータ

    private List<CoordinatorBase> m_children;   //子コーディネータ

    #region コンストラクタ

    public CoordinatorBase() {
        m_members = new List<I_FactionMember>();
        m_tupleSpace = new TupleSpace();
        m_parent = null;
        m_children = new List<CoordinatorBase>();
    }

    #endregion

    /// <summary>
    /// インスタンス生成時に一度だけ呼ぶ処理
    /// </summary>
    public abstract void OnCreate();

    /// <summary>
    /// 開始処理
    /// </summary>
    public abstract void OnStart();

    /// <summary>
    /// 更新処理
    /// </summary>
    public abstract bool OnUpdate();

    /// <summary>
    /// 終了処理
    /// </summary>
    public abstract void OnExit();

    //--------------------------------------------------------------------------------------
    /// アクセッサ
    //--------------------------------------------------------------------------------------

    virtual public void AddMember(I_FactionMember member) { m_members.Add(member); }

    virtual public void RemoveMember(I_FactionMember member) { m_members.Remove(member); }

    public List<I_FactionMember> GetMembers() { return m_members; }

    public TupleSpace GetTupleSpace() { return m_tupleSpace; }

    public void SetParent(CoordinatorBase parent) {
        //nullでないなら、親にchildを設定する。
        if(parent != null) {
            parent.AddChild(this);  //親に子になったことを伝える
        }
        
        //nullでかつ、すでに親がいるなら
        if(parent == null && HasParent()) {
            m_parent.RemoveChild(this); //親に外れることを伝える
        }
        
        m_parent = parent; 
    }

    public CoordinatorBase GetParent() { return m_parent; }

    public bool HasParent() { return m_parent != null; }

    public void AddChild(CoordinatorBase child) { m_children.Add(child); }

    public void RemoveChild(CoordinatorBase child) { m_children.Remove(child); }
}


//--------------------------------------------------------------------------------------
/// ファクションコーディネータの子どもの基底クラス
//--------------------------------------------------------------------------------------
public abstract class FactionChildCoordinatorBase : CoordinatorBase
{
    private FactionCoordinator m_faction;

    public FactionChildCoordinatorBase(FactionCoordinator faction):
        base()
    {
        m_faction = faction;
    }

    public override void AddMember(I_FactionMember member)
    {
        base.AddMember(member);

        member.SetAssignedCoordinator(this);
        member.AssignCoordinatorEvent(this);
    }

    public override void RemoveMember(I_FactionMember member)
    {
        base.RemoveMember(member);

        member.SetAssignedCoordinator(null);
        member.UnsignCoordinatorEvent(this);
    }

    //--------------------------------------------------------------------------------------
    /// アクセッサ
    //--------------------------------------------------------------------------------------

    public FactionCoordinator GetFaction() { return m_faction; }
}
