using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//--------------------------------------------------------------------------------------
/// �R�[�f�B�l�[�^�̊��N���X
//--------------------------------------------------------------------------------------
public abstract class CoordinatorBase
{
    protected List<I_FactionMember> m_members;  //�t�@�N�V�����ɏ������郁���o�[

    private TupleSpace m_tupleSpace;            //�^�v���X�y�[�X

    private CoordinatorBase m_parent;           //�e�R�[�f�B�l�[�^

    private List<CoordinatorBase> m_children;   //�q�R�[�f�B�l�[�^

    #region �R���X�g���N�^

    public CoordinatorBase() {
        m_members = new List<I_FactionMember>();
        m_tupleSpace = new TupleSpace();
        m_parent = null;
        m_children = new List<CoordinatorBase>();
    }

    #endregion

    /// <summary>
    /// �C���X�^���X�������Ɉ�x�����Ăԏ���
    /// </summary>
    public abstract void OnCreate();

    /// <summary>
    /// �J�n����
    /// </summary>
    public abstract void OnStart();

    /// <summary>
    /// �X�V����
    /// </summary>
    public abstract bool OnUpdate();

    /// <summary>
    /// �I������
    /// </summary>
    public abstract void OnExit();

    //--------------------------------------------------------------------------------------
    /// �A�N�Z�b�T
    //--------------------------------------------------------------------------------------

    virtual public void AddMember(I_FactionMember member) { m_members.Add(member); }

    virtual public void RemoveMember(I_FactionMember member) { m_members.Remove(member); }

    public List<I_FactionMember> GetMembers() { return m_members; }

    public TupleSpace GetTupleSpace() { return m_tupleSpace; }

    public void SetParent(CoordinatorBase parent) {
        //null�łȂ��Ȃ�A�e��child��ݒ肷��B
        if(parent != null) {
            parent.AddChild(this);  //�e�Ɏq�ɂȂ������Ƃ�`����
        }
        
        //null�ł��A���łɐe������Ȃ�
        if(parent == null && HasParent()) {
            m_parent.RemoveChild(this); //�e�ɊO��邱�Ƃ�`����
        }
        
        m_parent = parent; 
    }

    public CoordinatorBase GetParent() { return m_parent; }

    public bool HasParent() { return m_parent != null; }

    public void AddChild(CoordinatorBase child) { m_children.Add(child); }

    public void RemoveChild(CoordinatorBase child) { m_children.Remove(child); }
}


//--------------------------------------------------------------------------------------
/// �t�@�N�V�����R�[�f�B�l�[�^�̎q�ǂ��̊��N���X
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
    /// �A�N�Z�b�T
    //--------------------------------------------------------------------------------------

    public FactionCoordinator GetFaction() { return m_faction; }
}
