using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputAccessController : MonoBehaviour
{
    private HashSet<I_InputAccess> m_inputAccessList = new HashSet<I_InputAccess>();
    private PlayerStator m_stator;

    [SerializeField]
    private float m_overRange = 5.0f;   //あまりにも遠いなら処理を省く

    private DoorLock m_currentAccessDoorLock = null;
    public DoorLock CurrentAccessDoorLock { 
        get => m_currentAccessDoorLock;
        set => m_currentAccessDoorLock = value;
    }

    private void Awake()
    {
        m_stator = GetComponent<PlayerStator>();
    }

    private void Update()
    {
        if (PlayerInputer.IsAccess())
        {
            foreach (var access in m_inputAccessList)
            {
                float toAccessRange = (access.GetGameObject().transform.position - transform.position).magnitude;
                if (toAccessRange < m_overRange) {
                    access?.Access(this.gameObject);    //アクセス
                    ChangeState(access);
                }
                else {
                    m_inputAccessList.Remove(access);
                }
            }
        }
    }

    private void ChangeState(I_InputAccess access)
    {
        var door = access as OpenDoor;
        if(door == null) {
            return;
        }

        m_currentAccessDoorLock = access.GetGameObject().GetComponent<DoorLock>();
        if (!m_currentAccessDoorLock.IsLock) {
            return;
        }

        if (m_stator.GetCurrentState() == PlayerStator.StateType.Normal) {
            m_stator.ChangeState(PlayerStator.StateType.DoorLock);
            m_currentAccessDoorLock = access.GetGameObject().GetComponent<DoorLock>();
            return;
        }

        if(m_stator.GetCurrentState() == PlayerStator.StateType.DoorLock) {
            return;
        }
    }

    public void AddInputAccess(I_InputAccess access)
    {
        m_inputAccessList.Add(access);
    }

    public void RemoveInputAccess(I_InputAccess access)
    {
        m_inputAccessList.Remove(access);
    }
}
