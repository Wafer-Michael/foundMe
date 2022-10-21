using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HandPoseDebug : MonoBehaviour
{
    HandPose.Pose pose;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        OVRCustomSkeleton skeleton = new OVRCustomSkeleton();
        pose = HandPose.GetPose(skeleton);
    }
}
