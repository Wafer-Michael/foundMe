using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputAccessController : MonoBehaviour
{
    private HashSet<I_InputAccess> m_inputAccessList = new HashSet<I_InputAccess>();
    private PlayerStator m_stator;

    [SerializeField]
    private float m_overRange = 5.0f;   //あまりにも遠いなら処理を省く

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
                if (toAccessRange < m_overRange)
                {
                    access?.Access(this.gameObject);    //アクセス
                    ChangeState(access);
                }
                else
                {
                    m_inputAccessList.Remove(access);
                }
            }
        }
    }

    private void ChangeState(I_InputAccess access)
    {
        //Debug.Log("★ChangeState");

        var door = access as OpenDoor;
        if(door == null) {
            //Debug.Log("★DoorNull");
            return;
        }

        if (m_stator.GetCurrentState() == PlayerStator.StateType.Normal) {
            //Debug.Log("★ChangeLock");
            m_stator.ChangeState(PlayerStator.StateType.DoorLock);
            return;
        }

        if(m_stator.GetCurrentState() == PlayerStator.StateType.DoorLock) {
            //Debug.Log("★ChangeNormal");
            //m_stator.ChangeState(PlayerStator.StateType.Normal);
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
