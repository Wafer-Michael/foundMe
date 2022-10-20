using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HandPose : MonoBehaviour
{
    public enum Pose
    {
        guu = 0x00,
        paa = 0x1F,
        kyoki = 0x06
    }

    /// <summary>
    /// 現在のポーズを取得する
    /// </summary>
    public Pose GetPose(OVRCustomSkeleton skeleton)
    {
        System.Byte pose = 0x00; // 曲がっている指

        if (HandInputer.IsThumbStraight(skeleton))
        {
            pose += 0x01;
        }
        if (HandInputer.IsIndexStraight(skeleton))
        {
            pose += 0x02;
        }
        if (HandInputer.IsMiddleStraight(skeleton))
        {
            pose += 0x04;
        }
        if (HandInputer.IsRingStraight(skeleton))
        {
            pose += 0x08;
        }
        if (HandInputer.IsPinkyStraight(skeleton))
        {
            pose += 0x10;
        }

        return (Pose)pose;
    }

    /// <summary>
    /// そのポーズを取っているかを判定する
    /// </summary>
    /// <param name="pose">判定したいポーズ</param>
    public bool IsPose(Pose pose, OVRCustomSkeleton skeleton)
    {
        bool result = false;

        if (pose == GetPose(skeleton))
        {
            result = true;
        }
        return result;
    }
}
