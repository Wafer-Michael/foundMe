using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Oculus.Interaction.Grab;
using Oculus.Interaction.GrabAPI;
using Oculus.Interaction.Input;

public class Tester_DebugGrabObjects : MonoBehaviour
{
    [SerializeField]
    TMPro.TextMeshProUGUI m_debugText;    //DebugLender用のテクスチャ

    [SerializeField]
    Oculus.Interaction.HandGrab.HandGrabInteractor m_handGrabInteractor;

    [SerializeField]
    Oculus.Interaction.GrabStrengthIndicator m_grabStrengthIndicator;

    private void Awake()
    {
        if (!m_debugText)
        {
            m_debugText = GetComponent<TMPro.TextMeshProUGUI>();
        }
    }

    void Start()
    {
        
    }

    void Update()
    {
        //m_debugText.text = m_handGrabInteractor.DebugCount.ToString();

        m_debugText.text = m_grabStrengthIndicator.Count.ToString();
    }
}
