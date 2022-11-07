using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HandCreateObjectController : MonoBehaviour
{
    GameObject m_rightControlObject = null; //自分管理化にあるオブジェクト
    public GameObject RightControlObject {
        get { return m_rightControlObject; } 
        set
        {
            m_rightControlObject = value;
            FixedObjectUpdate();
            m_handVelocityController.SetVelocity(Vector3.zero);
        }
    }

    [SerializeField]
    OVRHand m_rightHand;
    OVRCustomSkeleton m_rightSkeleton;

    HandVectorController m_handVelocityController;

    [SerializeField]
    Vector3 m_offset = new Vector3(0.0f, 1.0f, 0.0f);

    private void Awake()
    {
        m_handVelocityController = m_rightHand.GetComponent<HandVectorController>();
    }

    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(RightControlObject == null)
        {
            return;
        }

        var velocity = m_handVelocityController.Velocity;
        if(velocity != Vector3.zero)
        {
            RightControlObject.GetComponent<Rigidbody>().AddForce(velocity * 100.0f);
        }
        
        //FixedObjectUpdate();

        EndControl();
    }

    void FixedObjectUpdate()
    {
        RightControlObject.transform.position = m_rightHand.PointerPose.position + m_offset + Camera.main.transform.position;
    }

    void EndControl()
    {
        OVRHand.HandFinger[] fingers = {
            OVRHand.HandFinger.Thumb,
            OVRHand.HandFinger.Index,
        };

        if (PlayerInputer.IsFingerPinch(m_rightHand, fingers))
        {
            RightControlObject.GetComponent<Rigidbody>().velocity = (m_rightHand.PointerPose.transform.forward * 10.0f);
            RightControlObject = null;
        }
    }
}
