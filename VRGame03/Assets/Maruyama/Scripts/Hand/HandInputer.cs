using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//using Microsoft.MixedReality.Toolkit;
//using Microsoft.MixedReality.Toolkit.Input;
//using Microsoft.MixedReality.Toolkit.Utilities;

//"C:\Program Files\Oculus\Support\oculus-client\OculusClient.exe"

public class HandInputer : MonoBehaviour//, IMixedRealityHandJointHandler//, IMixedRealitySourceStateHandler
{
    /// <summary>
    /// �W�����P���̏�Ԏ��
    /// </summary>
    [System.Serializable]
    public enum HandRockPaperScissorsStatus
    {
        Paper,      //�p�[
        Scissor,    //�`���L
        Rock,       //�O�[
        Nothing,
    }

    /// <summary>
    /// ����~���l
    /// 0.35 �F�G�f�B�^�[��̃N���b�N����ł̌��E�~���l
    /// </summary>
    [SerializeField, Range(-1, 1)]
    private float m_grabThreshold = 0.35f;

    // Inspector�œ��e���m�F���邽�߁A
    // Dictionary�ł͂Ȃ�List�ŕێ�����
    /// <summary>
    /// �W�F�X�`���[���
    /// </summary>
    //[SerializeField, Tooltip("�W�F�X�`���[���")]
    //private List<GestureState> p_GestureStateList;


}
