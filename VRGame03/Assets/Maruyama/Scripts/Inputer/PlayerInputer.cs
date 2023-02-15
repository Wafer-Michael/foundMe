using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInputer : MonoBehaviour
{
    [SerializeField]
    OVRHand m_ovrHand;

    static bool m_isRigftCrossButtom = false;
    static bool m_isLeftCrossButtom = false;
    static bool m_isDownCrossButtom = false;
    static bool m_isUpCrossButtom = false;

    /// <summary>
    /// �L�[�{�[�h����̈ړ����͏���
    /// </summary>
    /// <returns>�ړ��͂�Ԃ�</returns>
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
    /// �ړ��������擾
    /// </summary>
    /// <returns>�ړ�����</returns>
    static public Vector3 CalculateMoveVector()
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
    /// �߂��̃A�C�e���擾
    /// </summary>
    /// <returns></returns>
    static public bool IsTakeNearItem()
    {
        return Input.GetKeyDown(KeyCode.F) || Input.GetKeyDown("joystick button 0");
    }

    /// <summary>
    /// �o�b�e���[�����[�h
    /// </summary>
    /// <returns></returns>
    static public bool IsBatteryCharge()
    {
        //X�{�^��
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
    /// ���b�N�n�ɃA�N�Z�X����B
    /// </summary>
    /// <returns></returns>
    static public bool IsAccess()
    {
        //A�{�^��
        return Input.GetKeyDown(KeyCode.F) || Input.GetKeyDown("joystick button 0");
    }

    /// <summary>
    /// ���肷��B
    /// </summary>
    /// <returns></returns>
    static public bool IsEnter()
    {
        //A�{�^��
        return Input.GetKeyDown(KeyCode.Space) || Input.GetKeyDown("joystick button 2");
    }

    /// <summary>
    /// UI�N���[�Y�{�^��
    /// </summary>
    /// <returns></returns>
    static public bool IsClose() {
        //B�{�^��
        return Input.GetKeyDown(KeyCode.Q) || Input.GetKeyDown("joystick button 1");
    }
    
    /// <summary>
    /// �E����
    /// </summary>
    /// <returns></returns>
    static public bool IsRightDown() {
        var input = Input.GetAxis("CrossHorizontal");
        bool isDown = Input.GetKeyDown(KeyCode.RightArrow) || input >= 1.0f;
        Debug.Log("��������" + (Input.GetKeyDown(KeyCode.RightArrow) || input >= 1.0f) + "��������");
        Debug.Log("��������" + m_isRigftCrossButtom + "��������");
        if(!m_isRigftCrossButtom && isDown)
        {
            m_isRigftCrossButtom = true;
            return true;
        }

        m_isRigftCrossButtom = isDown;

        return false;
    }    

    /// <summary>
    /// ������
    /// </summary>
    /// <returns></returns>
    static public bool IsLeftDown() {
        var input = Input.GetAxis("CrossHorizontal"); Debug.Log(input);
        bool isDown = Input.GetKeyDown(KeyCode.LeftArrow) || input <= -1.0f;

        if (!m_isLeftCrossButtom && isDown)
        {
            m_isLeftCrossButtom = true;
            return true;
        }

        m_isLeftCrossButtom = isDown;

        return false;

    }

    /// <summary>
    /// �����
    /// </summary>
    /// <returns></returns>
    static public bool IsUpDown() {
        var input = Input.GetAxis("CrossVirtical");
        bool isDown = Input.GetKeyDown(KeyCode.UpArrow) || input >= 1.0f;

        if (!m_isUpCrossButtom && isDown)
        {
            m_isUpCrossButtom = true;
            return true;
        }

        m_isUpCrossButtom = isDown;

        return false;

    }

    /// <summary>
    /// ������
    /// </summary>
    /// <returns></returns>
    static public bool IsDownDown() {
        var input = Input.GetAxis("CrossVirtical");
        bool isDown = Input.GetKeyDown(KeyCode.DownArrow)||  input <= -1.0f;

        if (!m_isDownCrossButtom && isDown)
        {
            m_isDownCrossButtom = true;
            return true;
        }

        m_isDownCrossButtom = isDown;

        return false;

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
    ///	�e�X�g����
    //--------------------------------------------------------------------------------------

    /// <summary>
    /// �e�X�g�_���[�W����
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
    /// �f�o�b�O�p
    /// </summary>
    /// <param name="key"></param>
    /// <returns></returns>
    static public bool IsDebugKeyDown(KeyCode key)
    {
        return false;
        return Input.GetKeyDown(key);
    }

}
