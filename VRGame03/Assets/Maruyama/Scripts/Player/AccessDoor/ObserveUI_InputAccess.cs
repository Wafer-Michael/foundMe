using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UniRx;

public class ObserveUI_InputAccess : MonoBehaviour
{
    [SerializeField]
    private TMPro.TextMeshProUGUI m_accessUI;       //�A�N�Z�X����UI

    [SerializeField]
    private List<InputAccessTrigger> m_accessTriggers;   //�A�N�Z�XTrigger �����ׂĎ擾����

    private void Awake()
    {
        var triggers = FindObjectsOfType<InputAccessTrigger>();
        m_accessTriggers = new List<InputAccessTrigger>(triggers);

        foreach(var trigger in triggers)
        {
            
        }
    }
}
