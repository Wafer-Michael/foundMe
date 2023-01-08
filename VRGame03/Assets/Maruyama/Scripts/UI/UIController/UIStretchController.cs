using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Oculus.Interaction;

/// <summary>
/// UI���������΂����肷�鏈��
/// </summary>
public class UIStretchController : MonoBehaviour
{
    /// <summary>
    /// �f�t�H���g�p�����[�^
    /// </summary>
    public static readonly Parametor DEFAULT_PARAMETOR = new Parametor()
    {
        minSizeRatio = 0.25f,
        maxSizeRatio = 1.0f,
        stretchType = StretchType.Horizontal
    };

    /// <summary>
    /// �������΂������^�C�v
    /// </summary>
    public enum StretchType { 
        Horizontal, //������
        Vertical,   //�c����
    }

    [System.Serializable]
    public struct Parametor
    {
        public float minSizeRatio;      //�ŏ��T�C�Y�̊���
        public float maxSizeRatio;      //�ő�T�C�Y�̊���
        public StretchType stretchType; //�������΂����������^�C�v
    }

    [SerializeField]
    private Parametor m_param = DEFAULT_PARAMETOR;  //�p�����[�^

    private Vector3 m_initializePosition;           //�����ʒu

    [SerializeField]
    private SpriteRenderer m_spriteRender;          //�X�v���C�g�����_�[

    [SerializeField]
    private BoxProximityField m_boxProximityField;  //UI�̓����蔻��t�B�[���h

    private void Awake()
    {
        if (IsNullFaild()) {   //�X�v���C�g�����_�[�����݂��Ȃ��Ȃ珈�������Ȃ��B
            Debug.Log("UIStretchController::Awake(): SpriteRender��null�ł��B");
            return;
        }

        m_initializePosition = m_spriteRender.transform.position;   //�����ʒu�ݒ�
    }

    public void Tocuch_Mover(PointerEvent pointer)
    {
        if (IsNullFaild()) {   //�X�v���C�g�����_�[�����݂��Ȃ��Ȃ珈�������Ȃ��B
            return;
        }

        var size = CalculateSize(pointer);
        //Debug.Log("��" + size.ToString());
        m_spriteRender.size = size;
    }

    /// <summary>
    /// �T�C�Y���擾����B
    /// </summary>
    /// <param name="pointer">�^�b�`�|�C���^�[</param>
    /// <returns></returns>
    private Vector2 CalculateSize(in PointerEvent pointer)
    {
        //var range = CalculateInitializeToCurrentRange(pointer);
        var ratio = CalculatePositionRatio(pointer);
        var clampRatio = Mathf.Clamp(ratio, MinSizeRatio, MaxSizeRatio);
        Debug.Log("clampRatio: " + m_boxProximityField.transform.localScale.x.ToString());
        var range = clampRatio * m_boxProximityField.transform.localScale.x;
        var currentSize = m_spriteRender.size;  //���݂̃T�C�Y

        Vector2 size = GetStretchType() switch
        {
            StretchType.Horizontal => new Vector2(range, currentSize.y),
            StretchType.Vertical => new Vector2(currentSize.x, range),
            _ => m_spriteRender.size
        };

        return size;
    }

    /// <summary>
    /// �����ʒu���猻�݂̈ʒu�܂ł̋������v�Z���ĕԂ��B
    /// </summary>
    /// <returns>�����ʒu���猻�݂̈ʒu�܂ł̋���</returns>
    private float CalculateInitializeToCurrentRange(in PointerEvent pointer)
    {
        var toInitializeVec = m_initializePosition - pointer.Pose.position;
        return toInitializeVec.magnitude;
    }

    /// <summary>
    /// ���݃^�b�`�����ʒu���A�ǂ̈ʒu�ɂ��邩�������ŕԂ��B
    /// </summary>
    /// <returns></returns>
    private float CalculatePositionRatio(in PointerEvent pointer)
    {
        var scale = m_boxProximityField.transform.lossyScale;
        var startPosition = CalculateFieldLeftPosition();
        var range = pointer.Pose.position.x - startPosition.x;

        return range / scale.x;
    }

    /// <summary>
    /// �t�B�[���h�̍��[�̈ʒu���擾
    /// </summary>
    /// <returns></returns>
    private Vector3 CalculateFieldLeftPosition()
    {
        var position = m_boxProximityField.transform.position;
        var halfSize = m_boxProximityField.transform.lossyScale.x * 0.5f;
        return position + -Vector3.right * halfSize;
    }

    //--------------------------------------------------------------------------------------
    /// �A�N�Z�b�T
    //--------------------------------------------------------------------------------------

    public float MinSizeRatio
    {
        set => m_param.minSizeRatio = value;
        get => m_param.minSizeRatio;
    }

    public float MaxSizeRatio
    {
        set => m_param.maxSizeRatio = value;
        get => m_param.maxSizeRatio;
    }

    public void SetStretchType(StretchType type) { m_param.stretchType = type; }

    public StretchType GetStretchType() { return m_param.stretchType; }

    public Vector3 InitializePosition => m_initializePosition;

    /// <summary>
    /// ����̃I�u�W�F�N�g��null���ǂ���(null�Ȃ珈�����Ȃ�)
    /// </summary>
    /// <returns></returns>
    public bool IsNullFaild() {
        return !m_spriteRender || !m_boxProximityField;
    }
}
