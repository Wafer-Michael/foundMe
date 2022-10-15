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
    TMPro.TextMeshProUGUI m_debugText;    //DebugLender�p�̃e�N�X�`��

    const float DEFAULT_THRESHOLD = 0.8f;  //�w�̋Ȃ����̓��l(1�ɋ߂����قǌ������B)

    private void Update()
    {
        //�f�o�b�O
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
    /// �w�肵���S�Ă�BoneID��������ɂ��邩�ǂ������ׂ�
    /// </summary>
    /// <param name="threshold">臒l 1�ɋ߂��قǌ�����</param>
    /// <param name="boneids">�m�F�������{�[���̔z��</param>
    /// <returns></returns>
    static private bool IsStraight(OVRCustomSkeleton skeleton, float threshold, params OVRSkeleton.BoneId[] boneids)
    {
        if (boneids.Length < 3) { return false; }   //�{�[�������Ȃ��ƒ��ׂ悤���Ȃ�����false�B

        Vector3? oldVec = null; //��x�ڂ̃��[�v�͑��݂��Ȃ����߁B
        var dot = 1.0f;

        for (var index = 0; index < boneids.Length - 1; index++)
        {
            var v = (skeleton.CustomBones[(int)boneids[index + 1]].position - skeleton.CustomBones[(int)boneids[index]].position).normalized;
            if (oldVec.HasValue)    //�O�̃x�N�g�������݂���Ȃ�B
            {
                dot *= Vector3.Dot(v, oldVec.Value); //���ς̒l�𑍏悵�Ă����B
            }
            oldVec = v;//�ЂƂO�̎w�x�N�g���B
        }
        return dot >= threshold; //�w�肵��BoneID�̓��ς̑��悪臒l�𒴂��Ă����璼���Ƃ݂Ȃ��B
    }

    /// <summary>
    /// �e�w���Ȃ����Ă��邩�ǂ���
    /// </summary>
    /// <param name="skeleton">�X�P���g��</param>
    /// <param name="threshold">臒l 1�ɋ߂��قǌ�����</param>
    /// <returns>�^�������Ȃ�true</returns>
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
    /// �l�����w���Ȃ����Ă��邩�ǂ���
    /// </summary>
    /// <param name="skeleton">�X�P���g��</param>
    /// <param name="threshold">臒l 1�ɋ߂��قǌ�����</param>
    /// <returns>�^�������Ȃ�true</returns>
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
    /// ���w���Ȃ����Ă��邩�ǂ���
    /// </summary>
    /// <param name="skeleton">�X�P���g��</param>
    /// <param name="threshold">臒l 1�ɋ߂��قǌ�����</param>
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
    /// ��w���Ȃ����Ă��邩�ǂ���
    /// </summary>
    /// <param name="skeleton">�X�P���g��</param>
    /// <param name="threshold">臒l 1�ɋ߂��قǌ�����</param>
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
    /// ���w���Ȃ����Ă��邩�ǂ���
    /// </summary>
    /// <param name="skeleton">�X�P���g��</param>
    /// <param name="threshold">臒l 1�ɋ߂��قǌ�����</param>
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
