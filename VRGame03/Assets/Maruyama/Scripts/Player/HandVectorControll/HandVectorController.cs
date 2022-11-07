using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HandVectorController : MonoBehaviour
{ 
    [SerializeField]
    TMPro.TextMeshProUGUI m_debugText;

    OVRHand m_hand;

    //�O�t���[���̃|�W�V�������L�^
    Vector3 m_beforePosition;

    //���x
    Vector3 m_velocity;
    public Vector3 Velocity => m_velocity;
    public void SetVelocity(Vector3 velocity) { m_velocity = velocity; } 


    private void Awake()
    {
        m_hand = GetComponent<OVRHand>();
    }

    void Start()
    {
        
    }

    void Update()
    {
        Debug.Log(m_velocity);

        m_debugText.text = m_velocity.ToString();
    }

    private void LateUpdate()
    {
        if (!m_hand)
        {
            return;
        }

        m_velocity = m_hand.PointerPose.transform.position - m_beforePosition; //���x�ۑ�

        if(m_velocity == Vector3.zero)
        {
            m_velocity.Normalize();
        }

        m_beforePosition = m_hand.PointerPose.transform.position;  //�O�t���[���̑��x���Ǘ�
    }


}
