using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInputer : MonoBehaviour
{
    [SerializeField]
    OVRHand m_ovrHand;

    /// <summary>
    /// キーボードからの移動入力処理
    /// </summary>
    /// <returns>移動力を返す</returns>
    static private Vector3 CalculateKeyBoardMoveVector()
    {
        var moveVec = Vector3.zero;

        if (Input.GetKey(KeyCode.W))
        {
            moveVec += Vector3.forward;
        }

        if (Input.GetKey(KeyCode.S))
        {
            moveVec += Vector3.back;
        }

        if (Input.GetKey(KeyCode.A))
        {
            moveVec += Vector3.left;
        }

        if (Input.GetKey(KeyCode.D))
        {
            moveVec += Vector3.right;
        }

        return moveVec;
    }

    /// <summary>
    /// 移動方向を取得
    /// </summary>
    /// <returns>移動方向</returns>
    static public Vector3 CalculateMoveVector()
    {
        var stickVec = OVRInput.Get(OVRInput.RawAxis2D.LThumbstick);
        var moveVec = new Vector3(stickVec.x, 0, stickVec.y);
        moveVec += CalculateKeyBoardMoveVector();

        return moveVec.normalized;
    }

    /// <summary>
    /// マウスの移動ベクトルの取得
    /// </summary>
    /// <returns>マウスの移動ベクトル</returns>
    static public Vector3 CalculateMouseCameraMoveVec()
    {
        //var moveVec = new Vector3(-Input.GetAxis("Mouse Y"), Input.GetAxis("Mouse X"), 0.0f);
        var moveVec = new Vector3(0.0f, Input.GetAxis("Mouse X"), 0.0f);

        return moveVec;
    }

    static public bool IsSence()
    {
        return Input.GetKeyDown(KeyCode.P);
    }

    static public bool IsTakeNearItem()
    {
        return Input.GetKeyDown(KeyCode.F);
    }

    static public bool IsBatteryCharge()
    {
        return Input.GetKeyDown(KeyCode.R);
    }


    static public bool IsVRUIOpen()
    {
        return true;
    }

    static public bool IsVRUIClose()
    {
        return true;
    }

    static public bool IsDebugKeyDown(KeyCode key)
    {
        return Input.GetKeyDown(key);
    }

    static public bool IsAccess()
    {
        return Input.GetKeyDown(KeyCode.F);
    }

    static public bool IsClose() {
        return Input.GetKeyDown(KeyCode.Q);
    }

    /// <summary>
    /// つまむ行為
    /// </summary>
    /// <returns>つまむでいたらtrue</returns>
    public bool IsFingerPinch(params OVRHand.HandFinger[] handFingers)
    {
        foreach(var handFinger in handFingers) {
            if (!m_ovrHand.GetFingerIsPinching(handFinger)) {
                return false;
            }
        }

        return true;
    }

    static public bool IsFingerPinch(OVRHand hand, params OVRHand.HandFinger[] handFingers)
    {
        foreach (var handFinger in handFingers)
        {
            if (!hand.GetFingerIsPinching(handFinger))
            {
                return false;
            }
        }

        return true;
    }

    static public bool IsDebugPitch(OVRHand hand)
    {
        return hand.GetFingerIsPinching(OVRHand.HandFinger.Index) &&
            hand.GetFingerIsPinching(OVRHand.HandFinger.Thumb);
    }

    /// <summary>
    /// つまむ行為(人差し指と親指)
    /// </summary>
    /// <returns>つまんでいるならtrue</returns>
    public bool IsPinch()
    {
        return m_ovrHand.GetFingerIsPinching(OVRHand.HandFinger.Index) &&
            m_ovrHand.GetFingerIsPinching(OVRHand.HandFinger.Thumb);
    }

    /// <summary>
    /// グーポーズ
    /// </summary>
    /// <returns></returns>
    static public bool IsHandRock(OVRHand hand, float nearRange = 0.8f)
    {
        return hand.GetFingerPinchStrength(OVRHand.HandFinger.Thumb) >= nearRange;
    }

    /// <summary>
    /// 撃つボタン
    /// </summary>
    /// <returns></returns>
    static public bool IsShot()
    {
        return Input.GetMouseButton(0);
    }

    static public bool IsShotDown()
    {
        return Input.GetMouseButtonDown(0);
    }

    static public bool IsForceStopJack()
    {
        return true;
    }

    //--------------------------------------------------------------------------------------
    ///	テスト入力
    //--------------------------------------------------------------------------------------

    /// <summary>
    /// テストダメージ入力
    /// </summary>
    /// <returns></returns>
    static public bool IsTesterDamage()
    {
        return Input.GetMouseButtonDown(1);
    }

    static public bool IsChangeColor()
    {
        return Input.GetKeyDown(KeyCode.P);
    }

}
