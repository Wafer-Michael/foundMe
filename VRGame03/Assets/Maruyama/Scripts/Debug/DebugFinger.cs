using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DebugFinger : MonoBehaviour
{
    [SerializeField]
    private TMPro.TextMeshProUGUI m_debugText;

    [SerializeField]
    private OVRHand m_hand;

    private void Awake()
    {
        //m_hand = GetComponent<OVRHand>();
    }

    private void Update()
    {
        DebugDraw();
    }

    void DebugDraw()
    {
        string debugStr = "";

        OVRHand.HandFinger[] handFingers =
        {
            OVRHand.HandFinger.Thumb,
            OVRHand.HandFinger.Index,
            OVRHand.HandFinger.Middle,
            OVRHand.HandFinger.Ring,
            OVRHand.HandFinger.Pinky,
        };

        foreach (var finger in handFingers)
        {
            var strength = m_hand.GetFingerPinchStrength(finger);
            debugStr += finger.ToString() + ": " + strength.ToString() + "\n";
        }

        m_debugText.text = debugStr;
    }
}
