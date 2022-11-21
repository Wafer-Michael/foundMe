using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HandPose
{
    /// <summary>
    /// 手の形、伸びていたら1
    /// </summary>
    public enum Pose
    {
        guu = 0x00,
        paa = 0x1F,
        kyoki = 0x06,
        NONE = 0xFF
    }

    /// <summary>
    /// 現在のポーズを取得する
    /// </summary>
    static public Pose GetPose(OVRCustomSkeleton skeleton)
    {
        System.Byte pose = 0x00; // 曲がっている指

        // 伸びてる判定をとる
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

        // ポーズが存在するか判定する
        System.Byte result = 0xFF;
        foreach(Pose value in System.Enum.GetValues(typeof(Pose)))
        {
            if(pose == (System.Byte)value)
            {
                result = pose;
                break;
            }
        }

        return (Pose)result;
    }

    /// <summary>
    /// そのポーズを取っているかを判定する
    /// </summary>
    /// <param name="pose">判定したいポーズ</param>
    static public bool IsPose(Pose pose, OVRCustomSkeleton skeleton)
    {
        bool result = false;

        if (pose == GetPose(skeleton))
        {
            result = true;
        }
        return result;
    }
}
