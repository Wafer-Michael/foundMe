using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using System;
using System.Linq;

public abstract class StatorBase<OwnerType, EnumType, TransitionMember> : MonoBehaviour
    where OwnerType : class
    where EnumType : Enum
    where TransitionMember : struct
{
    [SerializeField]
    private TransitionMember m_transitionMember;                                  //条件分岐用のメンバー
    
    protected StateMachine<OwnerType, EnumType, TransitionMember> m_stateMachine; //ステートマシン

    protected virtual void Awake()
    {
        m_stateMachine = new StateMachine<OwnerType, EnumType, TransitionMember>(m_transitionMember);
    }

    private void Update()
    {
        m_stateMachine.OnUpdate();
    }

    /// <summary>
    /// ノードの生成
    /// </summary>
    protected abstract void CreateNode();

    /// <summary>
    /// エッジの生成
    /// </summary>
    protected abstract void CreateEdge();

    //--------------------------------------------------------------------------------------
    ///	アクセッサ
    //--------------------------------------------------------------------------------------

    /// <summary>
    /// ステートの変更
    /// </summary>
    /// <param name="state">ステート</param>
    /// <param name="priority">優先度</param> 
    public void ChangeState(EnumType state, int priority = 0)
    {
        m_stateMachine.ChangeState(state, priority);
    }

    /// <summary>
    /// ステートの強制変更
    /// </summary>
    /// <param name="state">ステート</param>
    public void ForceChangeState(EnumType state)
    {
        m_stateMachine.ForceChangeState(state);
    }

    public EnumType GetCurrentState()
    {
        return m_stateMachine.GetNowType();
    }

    public ref TransitionMember GetTransitionMember()
    {
        return ref m_stateMachine.GetTransitionStructMember();
    }

}
