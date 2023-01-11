using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class DecoratorBase<OwnerType> : I_Decorator
    where OwnerType : class
{
    private OwnerType m_owner;  //�I�[�i�[

    public DecoratorBase(OwnerType owner) {
        m_owner = owner;
    }

    virtual public void OnCreate() { }

    virtual public void ReserveCanTransition() { }
    virtual public void OnStart() { }
    virtual public void OnExit() { }

    public abstract bool CanTransition();

    public abstract bool CanUpdate();

    //--------------------------------------------------------------------------------------
    /// �A�N�Z�b�T
    //--------------------------------------------------------------------------------------

    public OwnerType GetOwner() { return m_owner; }
}
