using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorLockState : EnemyStateNodeBase<PlayerBase>
{
    private PlayerStator m_stator;
    private InputAccessController m_inputAccess;

    public DoorLockState(PlayerBase owner):
        base(owner)
    {
        m_stator = owner.GetComponent<PlayerStator>();
        m_inputAccess = owner.GetComponent<InputAccessController>();
    }

    protected override void ReserveChangeComponents()
    {
        base.ReserveChangeComponents();

        AddChangeComp(GetOwner().GetComponent<TesterMover>(), false, true);
        AddChangeComp(GetOwner().GetComponent<PCPlayer>(), false, true);
    }

    public override void OnStart()
    {
        base.OnStart();
    }

    public override bool OnUpdate()
    {
        if (PlayerInputer.IsClose()) {
            m_stator.ChangeState(PlayerStator.StateType.Normal);
            var currentLock = m_inputAccess.CurrentAccessDoorLock;
            if (currentLock) {
                currentLock.Interruption();
            }
        }

        return true;
    }

    public override void OnExit()
    {
        base.OnExit();
    }
}
