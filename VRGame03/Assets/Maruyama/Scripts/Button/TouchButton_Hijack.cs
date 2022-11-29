using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using OculusSampleFramework;

public class TouchButton_Hijack : TouchBottunEventBase
{
    [SerializeField]
    GameObject m_target;
    public GameObject Target => m_target;
    HijackController m_hijackController;

    [SerializeField]
    GameObject m_hijackTarget;  //ハイジャックのターゲット
    public GameObject HijackTarget => m_hijackTarget;

    public void Awake()
    {
        //nullだったらとりあえずPlayer
        if(Target == null)
        {
            m_target = FindObjectOfType<Player>().gameObject;
        }

        m_hijackController = Target.GetComponent<HijackController>();
    }

    public void Update()
    {
        
    }

    public override void Touch(InteractableStateArgs obj)
    {
        Hijack(); //ハイジャック
    }

    private void Hijack()
    {
        m_hijackController.StartHijack(HijackTarget);
    }
}
