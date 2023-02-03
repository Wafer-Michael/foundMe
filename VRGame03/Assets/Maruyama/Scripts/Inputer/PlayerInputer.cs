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

        return GetControllerInput();

        return moveVec;
    }

    static public Vector3 GetControllerInput()
    {
        var vertical = Input.GetAxisRaw("Vertical");
        var horizontal = Input.GetAxisRaw("Horizontal");

        return new Vector3(horizontal, 0.0f, vertical);
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
        var mouseAxis = Input.GetAxis("Mouse X");
        var stickAxis = Input.GetAxis("RHorizontal");

        var moveVec = new Vector3(0.0f, mouseAxis + stickAxis, 0.0f);

        return moveVec;
    }

    static public bool IsSence()
    {
        return false;
        return Input.GetKeyDown(KeyCode.P);
    }

    /// <summary>
    /// 近くのアイテム取得
    /// </summary>
    /// <returns></returns>
    static public bool IsTakeNearItem()
    {
        return Input.GetKeyDown(KeyCode.F) || Input.GetKeyDown("joystick button 0");
    }

    /// <summary>
    /// バッテリーリロード
    /// </summary>
    /// <returns></returns>
    static public bool IsBatteryCharge()
    {
        //Xボタン
        return Input.GetKeyDown(KeyCode.R) || Input.GetKeyDown("joystick button 2");
    }


    static public bool IsVRUIOpen()
    {
        return true;
    }

    static public bool IsVRUIClose()
    {
        return true;
    }

    /// <summary>
    /// ロック系にアクセスする。
    /// </summary>
    /// <returns></returns>
    static public bool IsAccess()
    {
        //Aボタン
        return Input.GetKeyDown(KeyCode.F) || Input.GetKeyDown("joystick button 0");
    }

    /// <summary>
    /// 決定する。
    /// </summary>
    /// <returns></returns>
    static public bool IsEnter()
    {
        //Aボタン
        return Input.GetKeyDown(KeyCode.Space) || Input.GetKeyDown("joystick button 2");
    }

    /// <summary>
    /// UIクローズボタン
    /// </summary>
    /// <returns></returns>
    static public bool IsClose() {
        //Bボタン
        return Input.GetKeyDown(KeyCode.Q) || Input.GetKeyDown("joystick button 1");
    }
    
    /// <summary>
    /// 右入力
    /// </summary>
    /// <returns></returns>
    static public bool IsRightDown() {
        var input = Input.GetAxis("CrossHorizontal");
        return Input.GetKeyDown(KeyCode.RightArrow) || input >= 1.0f;
    }    

    /// <summary>
    /// 左入力
    /// </summary>
    /// <returns></returns>
    static public bool IsLeftDown() {
        var input = Input.GetAxis("CrossHorizontal"); Debug.Log(input);
        return Input.GetKeyDown(KeyCode.LeftArrow) || input <= -1.0f;
    }    

    /// <summary>
    /// 上入力
    /// </summary>
    /// <returns></returns>
    static public bool IsUpDown() {
        var input = Input.GetAxis("CrossVirtical");
        return Input.GetKeyDown(KeyCode.UpArrow) || input >= 1.0f;
    }    

    /// <summary>
    /// 下入力
    /// </summary>
    /// <returns></returns>
    static public bool IsDownDown() {
        var input = Input.GetAxis("CrossVirtical");
        return Input.GetKeyDown(KeyCode.DownArrow)||  input <= -1.0f;
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
        return false;
        return Input.GetMouseButton(0);
    }

    static public bool IsShotDown()
    {
        return false;
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
        return false;
        return Input.GetMouseButtonDown(1);
    }

    static public bool IsChangeColor()
    {
        return false;
        return Input.GetKeyDown(KeyCode.P);
    }

    /// <summary>
    /// デバッグ用
    /// </summary>
    /// <param name="key"></param>
    /// <returns></returns>
    static public bool IsDebugKeyDown(KeyCode key)
    {
        return false;
        return Input.GetKeyDown(key);
    }

}
