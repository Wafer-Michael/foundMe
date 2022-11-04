using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputCreate : MonoBehaviour
{
    //HandPose m_handPose;

    [SerializeField]
    OVRHand m_rightHand;

    [SerializeField]
    GameObject m_prefab;

    [SerializeField]
    HandCreateObjectController m_handObjectController;

    void Start()
    {
        
    }

    void Update()
    {
        //�E��Ő���
        if(IsCreate(m_rightHand.GetComponent<OVRCustomSkeleton>())){
            var obj = Create();
            m_handObjectController.RightControlObject = obj;
        }
    }

    GameObject Create()
    {
        return Instantiate(m_prefab);
    }

    bool IsCreate(OVRCustomSkeleton skeleton)
    {
        if(m_handObjectController.RightControlObject != null)
        {
            return false;
        }

        //�肪�p�[�ŁA���A��������Ă�����
        var poseType = HandPose.GetPose(skeleton);
        if((poseType == HandPose.Pose.paa))
        {
            return true;
        }

        return false;
    }
}
