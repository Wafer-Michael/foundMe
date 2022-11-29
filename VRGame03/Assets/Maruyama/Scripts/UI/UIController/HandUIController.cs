using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HandUIController : MonoBehaviour
{
    [SerializeField]
    private List<VRUI> m_uis = new List<VRUI>();

    private OVRHand m_hand;
    private void Awake()
    {
        m_hand = GetComponent<OVRHand>();
    }
    private void Update()
    {
        if(PlayerInputer.IsVRUIOpenAndClose()){

        }
    }

    private void Open()
    {
        foreach(var ui in m_uis){
            ui.Open();
        }
    }

    private void Close()
    {
        foreach(var ui in m_uis)
        {
            ui.Close();
        }
    }
}
