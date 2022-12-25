using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class BehaviorTask : BehaviorNode
{
    
}

public abstract class BehaviorTaskBase<OwnerType> : BehaviorTask
    where OwnerType : class
{
    private OwnerType m_owner;  //�I�[�i�[

    public BehaviorTaskBase(OwnerType owner)
    {
        m_owner = owner;
    }

    /// <summary>
    /// �I�[�i�[�̎擾
    /// </summary>
    /// <returns>�I�[�i�[</returns>
    public OwnerType GetOwner() { return m_owner; }
}
