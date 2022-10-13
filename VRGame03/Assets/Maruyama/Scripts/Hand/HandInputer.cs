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
    /// ジャンケンの状態種別
    /// </summary>
    [System.Serializable]
    public enum HandRockPaperScissorsStatus
    {
        Paper,      //パー
        Scissor,    //チョキ
        Rock,       //グー
        Nothing,
    }

    /// <summary>
    /// 握り敷居値
    /// 0.35 ：エディター上のクリック動作での限界敷居値
    /// </summary>
    [SerializeField, Range(-1, 1)]
    private float m_grabThreshold = 0.35f;

    // Inspectorで内容を確認するため、
    // DictionaryではなくListで保持する
    /// <summary>
    /// ジェスチャー状態
    /// </summary>
    //[SerializeField, Tooltip("ジェスチャー状態")]
    //private List<GestureState> p_GestureStateList;


}
