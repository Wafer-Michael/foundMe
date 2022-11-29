using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIController : MonoBehaviour
{
    [SerializeField]
    private List<VRUI> m_uis = new List<VRUI>();
    
    

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
