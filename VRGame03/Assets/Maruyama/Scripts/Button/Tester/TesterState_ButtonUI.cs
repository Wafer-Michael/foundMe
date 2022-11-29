using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using OculusSampleFramework;

public class TesterState_ButtonUI : MonoBehaviour
{
    [SerializeField]
    TMPro.TextMeshProUGUI m_debugText;

    [SerializeField]
    OVRHand m_hand;

    private void Start()
    {
        m_debugText.text = "Start";
    }

    private void Update()
    {
        if (PlayerInputer.IsDebugPitch(m_hand))
        {
            m_debugText.text = "";
        }
    }

    public void Debug(InteractableStateArgs obj)
    {
        m_debugText.text += obj.NewInteractableState + ",";
    }
}
