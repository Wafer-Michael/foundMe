using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HandPoseDebug : MonoBehaviour
{
    private HandPose.Pose m_beforePose = HandPose.Pose.NONE;

    [SerializeField]
    OVRCustomSkeleton m_debugSkeleton;

    [SerializeField]
    TMPro.TextMeshProUGUI m_debugText;

    // Update is called once per frame
    void Update()
    {
        HandPose.Pose pose;
        pose = HandPose.GetPose(m_debugSkeleton);

        //Debug.Log("ÅöÅ`" + pose.ToString() + "ÅöÅ`");
        m_debugText.text = pose.ToString();

        if (pose != m_beforePose)
        {
            m_beforePose = pose;
        }
    }
}
