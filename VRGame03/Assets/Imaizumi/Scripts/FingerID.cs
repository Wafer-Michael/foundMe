using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FingerID : MonoBehaviour
{
    public readonly int[][] FingerId = new int[][]
    {
        new int[] { (int)OVRSkeleton.BoneId.Hand_Thumb2, (int)OVRSkeleton.BoneId.Hand_Thumb3, (int)OVRSkeleton.BoneId.Hand_ThumbTip },
        new int[] { (int)OVRSkeleton.BoneId.Hand_Index1, (int)OVRSkeleton.BoneId.Hand_Index2, (int)OVRSkeleton.BoneId.Hand_Index3, (int)OVRSkeleton.BoneId.Hand_IndexTip },
        new int[] { (int)OVRSkeleton.BoneId.Hand_Middle1, (int)OVRSkeleton.BoneId.Hand_Middle2, (int)OVRSkeleton.BoneId.Hand_Middle3, (int)OVRSkeleton.BoneId.Hand_MiddleTip },
        new int[] { (int)OVRSkeleton.BoneId.Hand_Ring1, (int)OVRSkeleton.BoneId.Hand_Ring2, (int)OVRSkeleton.BoneId.Hand_Ring3, (int)OVRSkeleton.BoneId.Hand_RingTip },
        new int[] { (int)OVRSkeleton.BoneId.Hand_Pinky1, (int)OVRSkeleton.BoneId.Hand_Pinky2, (int)OVRSkeleton.BoneId.Hand_Pinky3, (int)OVRSkeleton.BoneId.Hand_PinkyTip }
    };

    public enum Finger
    { 
        Thumb,
        Index,
        Middle,
        Ring,
        Pinky
    }

    public int[] GetFinger(Finger id)
    {
        int[] fingerID = FingerId[(int)id];

        return fingerID;
    }
}
