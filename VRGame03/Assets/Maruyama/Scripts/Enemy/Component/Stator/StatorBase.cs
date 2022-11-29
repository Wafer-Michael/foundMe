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
    private TransitionMember m_transitionMember;                                  //��������p�̃����o�[
    
    protected StateMachine<OwnerType, EnumType, TransitionMember> m_stateMachine; //�X�e�[�g�}�V��

    protected virtual void Awake()
    {
        m_stateMachine = new StateMachine<OwnerType, EnumType, TransitionMember>(m_transitionMember);
    }

    private void Update()
    {
        m_stateMachine.OnUpdate();
    }

    /// <summary>
    /// �m�[�h�̐���
    /// </summary>
    protected abstract void CreateNode();

    /// <summary>
    /// �G�b�W�̐���
    /// </summary>
    protected abstract void CreateEdge();

    //--------------------------------------------------------------------------------------
    ///	�A�N�Z�b�T
    //--------------------------------------------------------------------------------------

    /// <summary>
    /// �X�e�[�g�̕ύX
    /// </summary>
    /// <param name="state">�X�e�[�g</param>
    /// <param name="priority">�D��x</param> 
    public void ChangeState(EnumType state, int priority = 0)
    {
        m_stateMachine.ChangeState(state, priority);
    }

    /// <summary>
    /// �X�e�[�g�̋����ύX
    /// </summary>
    /// <param name="state">�X�e�[�g</param>
    public void ForceChangeState(EnumType state)
    {
        m_stateMachine.ForceChangeState(state);
    }

    public EnumType GetCurrentState()
    {
        return m_stateMachine.GetNowType();
    }

    public TransitionMember GetTransitionMember()
    {
        return m_stateMachine.GetTransitionStructMember();
    }

}
