using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorLockState : EnemyStateNodeBase<PlayerBase>
{
    private PlayerStator m_stator;

    public DoorLockState(PlayerBase owner):
        base(owner)
    {
        m_stator = owner.GetComponent<PlayerStator>();
    }

    protected override void ReserveChangeComponents()
    {
        base.ReserveChangeComponents();

        AddChangeComp(GetOwner().GetComponent<TesterMover>(), false, true);
    }

    public override void OnStart()
    {
        base.OnStart();
    }

    public override bool OnUpdate()
    {
        if (PlayerInputer.IsClose()) {
            m_stator.ChangeState(PlayerStator.StateType.Normal);
        }

        return true;
    }

    public override void OnExit()
    {
        base.OnExit();
    }
}
