using System.Collections;
using System.Collections.Generic;
using UnityEngine;

static class FingerID
{
    static private readonly OVRSkeleton.BoneId[][] FingerId = new OVRSkeleton.BoneId[][]
    {
        new OVRSkeleton.BoneId[] { OVRSkeleton.BoneId.Hand_Thumb2,  OVRSkeleton.BoneId.Hand_Thumb3,  OVRSkeleton.BoneId.Hand_ThumbTip },                                   // Thumb
        new OVRSkeleton.BoneId[] { OVRSkeleton.BoneId.Hand_Index1,  OVRSkeleton.BoneId.Hand_Index2,  OVRSkeleton.BoneId.Hand_Index3,  OVRSkeleton.BoneId.Hand_IndexTip },  // Index
        new OVRSkeleton.BoneId[] { OVRSkeleton.BoneId.Hand_Middle1, OVRSkeleton.BoneId.Hand_Middle2, OVRSkeleton.BoneId.Hand_Middle3, OVRSkeleton.BoneId.Hand_MiddleTip }, // Middle
        new OVRSkeleton.BoneId[] { OVRSkeleton.BoneId.Hand_Ring1,   OVRSkeleton.BoneId.Hand_Ring2,   OVRSkeleton.BoneId.Hand_Ring3,   OVRSkeleton.BoneId.Hand_RingTip },   // Ring
        new OVRSkeleton.BoneId[] { OVRSkeleton.BoneId.Hand_Pinky1,  OVRSkeleton.BoneId.Hand_Pinky2,  OVRSkeleton.BoneId.Hand_Pinky3,  OVRSkeleton.BoneId.Hand_PinkyTip }   // Pinky
    };

    public enum Finger
    { 
        Thumb,
        Index,
        Middle,
        Ring,
        Pinky
    }

    /// <summary>
    /// 指のIDを取得する
    /// </summary>
    /// <param name="id">取得したい指</param>
    /// <returns>指の骨の配列</returns>
    static public OVRSkeleton.BoneId[] GetFinger(Finger id)
    {
        OVRSkeleton.BoneId[] fingerID = FingerId[(int)id];

        return fingerID;
    }
}
