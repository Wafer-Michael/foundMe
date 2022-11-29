using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInputer : MonoBehaviour
{
    [SerializeField]
    OVRHand m_ovrHand;

    /// <summary>
    /// �L�[�{�[�h����̈ړ����͏���
    /// </summary>
    /// <returns>�ړ��͂�Ԃ�</returns>
    private Vector3 CalculateKeyBoardMoveVector()
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
    /// �ړ��������擾
    /// </summary>
    /// <returns>�ړ�����</returns>
    public Vector3 CalculateMoveVector()
    {
        var stickVec = OVRInput.Get(OVRInput.RawAxis2D.LThumbstick);
        var moveVec = new Vector3(stickVec.x, 0, stickVec.y);
        moveVec += CalculateKeyBoardMoveVector();

        return moveVec.normalized;
    }

    /// <summary>
    /// �}�E�X�̈ړ��x�N�g���̎擾
    /// </summary>
    /// <returns>�}�E�X�̈ړ��x�N�g��</returns>
    public Vector3 CalculateMouseCameraMoveVec()
    {
        //var moveVec = new Vector3(-Input.GetAxis("Mouse Y"), Input.GetAxis("Mouse X"), 0.0f);
        var moveVec = new Vector3(0.0f, Input.GetAxis("Mouse X"), 0.0f);

        return moveVec;
    }

    static public bool IsVRUIOpenAndClose()
    {
        return true;
    }

    /// <summary>
    /// �܂ލs��
    /// </summary>
    /// <returns>�܂ނł�����true</returns>
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
    /// �܂ލs��(�l�����w�Ɛe�w)
    /// </summary>
    /// <returns>�܂�ł���Ȃ�true</returns>
    public bool IsPinch()
    {
        return m_ovrHand.GetFingerIsPinching(OVRHand.HandFinger.Index) &&
            m_ovrHand.GetFingerIsPinching(OVRHand.HandFinger.Thumb);
    }

    /// <summary>
    /// �O�[�|�[�Y
    /// </summary>
    /// <returns></returns>
    static public bool IsHandRock(OVRHand hand, float nearRange = 0.8f)
    {
        return hand.GetFingerPinchStrength(OVRHand.HandFinger.Thumb) >= nearRange;
    }

    /// <summary>
    /// ���{�^��
    /// </summary>
    /// <returns></returns>
    static public bool IsShot()
    {
        return Input.GetMouseButton(0);
    }

    //--------------------------------------------------------------------------------------
    ///	�e�X�g����
    //--------------------------------------------------------------------------------------

    /// <summary>
    /// �e�X�g�_���[�W����
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
