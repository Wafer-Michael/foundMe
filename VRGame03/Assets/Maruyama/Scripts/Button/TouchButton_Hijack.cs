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
    GameObject m_hijackTarget;  //�n�C�W���b�N�̃^�[�Q�b�g
    public GameObject HijackTarget => m_hijackTarget;

    public void Awake()
    {
        //null��������Ƃ肠����Player
        if(Target == null)
        {
            m_target = FindObjectOfType<PlayerBase>().gameObject;
        }

        m_hijackController = Target.GetComponent<HijackController>();
    }

    public void Update()
    {
        
    }

    public void Open()
    {
        //gameObject.SetActive(true);
    }

    public void Close()
    {
        //gameObject.SetActive(false);
    }

    public override void Touch(InteractableStateArgs obj)
    {
        Hijack(); //�n�C�W���b�N
    }

    private void Hijack()
    {
        m_hijackController.StartHijack(HijackTarget);
    }
}
