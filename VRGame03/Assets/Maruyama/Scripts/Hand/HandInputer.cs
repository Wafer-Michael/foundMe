using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//using Microsoft.MixedReality.Toolkit;
//using Microsoft.MixedReality.Toolkit.Input;
//using Microsoft.MixedReality.Toolkit.Utilities;

//"C:\Program Files\Oculus\Support\oculus-client\OculusClient.exe"

public class HandInputer : MonoBehaviour//, IMixedRealityHandJointHandler//, IMixedRealitySourceStateHandler
{

    [SerializeField]
    OVRCustomSkeleton m_debugSkeleton;

    [SerializeField]
    TMPro.TextMeshProUGUI m_debugText;    //DebugLender用のテクスチャ

    const float DEFAULT_THRESHOLD = 0.8f;  //指の曲がり具合の闘値(1に近しいほど厳しい。)

    private void Update()
    {
        //デバッグ
        if (IsIndexStraight(m_debugSkeleton))
        {
            m_debugText.text = "Straight";
        }
        else
        {
            m_debugText.text = "Magaru";
        }
    }

    /// <summary>
    /// 指定した全てのBoneIDが直線状にあるかどうか調べる
    /// </summary>
    /// <param name="threshold">閾値 1に近いほど厳しい</param>
    /// <param name="boneids">確認したいボーンの配列</param>
    /// <returns></returns>
    static private bool IsStraight(OVRCustomSkeleton skeleton, float threshold, params OVRSkeleton.BoneId[] boneids)
    {
        if (boneids.Length < 3) { return false; }   //ボーンが少ないと調べようがないためfalse。

        Vector3? oldVec = null; //一度目のループは存在しないため。
        var dot = 1.0f;

        for (var index = 0; index < boneids.Length - 1; index++)
        {
            var v = (skeleton.CustomBones[(int)boneids[index + 1]].position - skeleton.CustomBones[(int)boneids[index]].position).normalized;
            if (oldVec.HasValue)    //前のベクトルが存在するなら。
            {
                dot *= Vector3.Dot(v, oldVec.Value); //内積の値を総乗していく。
            }
            oldVec = v;//ひとつ前の指ベクトル。
        }
        return dot >= threshold; //指定したBoneIDの内積の総乗が閾値を超えていたら直線とみなす。
    }

    /// <summary>
    /// 親指が曲がっているかどうか
    /// </summary>
    /// <param name="skeleton">スケルトン</param>
    /// <param name="threshold">閾値 1に近いほど厳しい</param>
    /// <returns>真っすぐならtrue</returns>
    static public bool IsThumbStraight(OVRCustomSkeleton skeleton, float threshold = DEFAULT_THRESHOLD)
    {
        OVRSkeleton.BoneId[] boneids = {
            OVRSkeleton.BoneId.Hand_Thumb2,
            OVRSkeleton.BoneId.Hand_Thumb3,
            OVRSkeleton.BoneId.Hand_ThumbTip
        };

        return IsStraight(skeleton, threshold, boneids);
    }

    /// <summary>
    /// 人差し指が曲がっているかどうか
    /// </summary>
    /// <param name="skeleton">スケルトン</param>
    /// <param name="threshold">閾値 1に近いほど厳しい</param>
    /// <returns>真っすぐならtrue</returns>
    static public bool IsIndexStraight(OVRCustomSkeleton skeleton, float threshold = DEFAULT_THRESHOLD)
    {
        OVRSkeleton.BoneId[] boneids = {
            OVRSkeleton.BoneId.Hand_Index1,
            OVRSkeleton.BoneId.Hand_Index2,
            OVRSkeleton.BoneId.Hand_Index3,
            OVRSkeleton.BoneId.Hand_IndexTip
        };

        return IsStraight(skeleton, threshold, boneids);
    }

    /// <summary>
    /// 中指が曲がっているかどうか
    /// </summary>
    /// <param name="skeleton">スケルトン</param>
    /// <param name="threshold">閾値 1に近いほど厳しい</param>
    /// <returns></returns>
    static public bool IsMiddleStraight(OVRCustomSkeleton skeleton, float threshold = DEFAULT_THRESHOLD)
    {
        OVRSkeleton.BoneId[] boneids = {
            OVRSkeleton.BoneId.Hand_Middle1,
            OVRSkeleton.BoneId.Hand_Middle2,
            OVRSkeleton.BoneId.Hand_Middle3,
            OVRSkeleton.BoneId.Hand_MiddleTip
        };

        return IsStraight(skeleton, threshold, boneids);
    }

    /// <summary>
    /// 薬指が曲がっているかどうか
    /// </summary>
    /// <param name="skeleton">スケルトン</param>
    /// <param name="threshold">閾値 1に近いほど厳しい</param>
    /// <returns></returns>
    static public bool IsRingStraight(OVRCustomSkeleton skeleton, float threshold = DEFAULT_THRESHOLD)
    {
        OVRSkeleton.BoneId[] boneids = {
            OVRSkeleton.BoneId.Hand_Ring1,
            OVRSkeleton.BoneId.Hand_Ring2,
            OVRSkeleton.BoneId.Hand_Ring3,
            OVRSkeleton.BoneId.Hand_RingTip
        };

        return IsStraight(skeleton, threshold, boneids);
    }

    /// <summary>
    /// 小指が曲がっているかどうか
    /// </summary>
    /// <param name="skeleton">スケルトン</param>
    /// <param name="threshold">閾値 1に近いほど厳しい</param>
    /// <returns></returns>
    static public bool IsPinkyStraight(OVRCustomSkeleton skeleton, float threshold = DEFAULT_THRESHOLD)
    {
        OVRSkeleton.BoneId[] boneids = {
            OVRSkeleton.BoneId.Hand_Pinky1,
            OVRSkeleton.BoneId.Hand_Pinky2,
            OVRSkeleton.BoneId.Hand_Pinky3,
            OVRSkeleton.BoneId.Hand_PinkyTip
        };

        return IsStraight(skeleton, threshold, boneids);
    }

}
