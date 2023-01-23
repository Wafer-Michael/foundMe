using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputAccessController : MonoBehaviour
{
    private HashSet<I_InputAccess> m_inputAccessList = new HashSet<I_InputAccess>();

    [SerializeField]
    private float m_overRange = 5.0f;   //Ç†Ç‹ÇËÇ…Ç‡âìÇ¢Ç»ÇÁèàóùÇè»Ç≠

    private void Update()
    {
        if (PlayerInputer.IsAccess())
        {
            foreach (var access in m_inputAccessList)
            {
                float toAccessRange = (access.GetGameObject().transform.position - transform.position).magnitude;
                if (toAccessRange < m_overRange)
                {
                    access?.Access(this.gameObject);
                }
                else
                {
                    m_inputAccessList.Remove(access);
                }
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        //var access = other.GetComponent<I_InputAccess>();
        //if(access == null)
        //{
        //    return;
        //}

        //m_inputAccessList.Add(access);
    }

    private void OnTriggerExit(Collider other)
    {
        //var access = other.GetComponent<I_InputAccess>();
        //if (access == null)
        //{
        //    return;
        //}

        //m_inputAccessList.Remove(access);
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
