using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class BehaviorTask : BehaviorNode
{
    
}

public abstract class BehaviorTaskBase<OwnerType> : BehaviorTask
    where OwnerType : class
{
    private OwnerType m_owner;  //オーナー

    public BehaviorTaskBase(OwnerType owner)
    {
        m_owner = owner;
    }

    /// <summary>
    /// オーナーの取得
    /// </summary>
    /// <returns>オーナー</returns>
    public OwnerType GetOwner() { return m_owner; }
}
