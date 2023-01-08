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

    [SerializeField]
    private bool m_isInitializeMinSize = false;     //�����ݒ�ōŏ��T�C�Y�ɍ��킹�邩�ǂ���

    private void Awake()
    {
        if (IsNullFaild()) {   //�X�v���C�g�����_�[�����݂��Ȃ��Ȃ珈�������Ȃ��B
            Debug.Log("UIStretchController::Awake(): SpriteRender��null�ł��B");
            return;
        }

        m_initializePosition = m_spriteRender.transform.position;   //�����ʒu�ݒ�

        //�����ݒ�ōŏ��T�C�Y�ɍ��킹��Ȃ�B
        if (m_isInitializeMinSize) {
            var size = CalculateSize(m_param.minSizeRatio);
            m_spriteRender.size = size;
        }
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
        var ratio = CalculatePositionRatio_Clamp(pointer);
        return CalculateSize(ratio);
    }

    /// <summary>
    /// �T�C�Y���擾����
    /// </summary>
    /// <param name="ratio">�傫���̊���</param>
    /// <returns></returns>
    private Vector2 CalculateSize(float ratio)
    {
        var range = ratio * GetMaxLocalRange();
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
    /// ���݃^�b�`�����ʒu���A�ǂ̈ʒu�ɂ��邩�������ŕԂ��B
    /// </summary>
    /// <returns></returns>
    private float CalculatePositionRatio(in PointerEvent pointer)
    {
        var maxRange = GetMaxLossyRange();

        Vector3 startPosition = GetStretchType() switch {
            StretchType.Horizontal => CalculateFieldLeftPosition(),
            StretchType.Vertical => CalculateFieldUpPosition(),
            _ => Vector3.zero
        };

        var range = GetStretchType() switch { 
            StretchType.Horizontal => pointer.Pose.position.x - startPosition.x,
            StretchType.Vertical => pointer.Pose.position.y - startPosition.y,
            _ => 0.0f
        };

        return range / maxRange;
    }

    /// <summary>
    /// ���݃^�b�`�����ʒu���A�ǂ̈ʒu�ɂ��邩���N�����v���Ċ����ŕԂ��B
    /// </summary>
    /// <param name="pointer"></param>
    /// <returns></returns>
    public float CalculatePositionRatio_Clamp(in PointerEvent pointer)
    {
        float ratio = CalculatePositionRatio(pointer);
        return Mathf.Clamp(ratio, MinSizeRatio, MaxSizeRatio);
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

    /// <summary>
    /// �t�B�[���h�̏�[�̈ʒu���擾
    /// </summary>
    /// <returns></returns>
    private Vector3 CalculateFieldUpPosition()
    {
        var position = m_boxProximityField.transform.position;
        var halfSize = m_boxProximityField.transform.lossyScale.y * 0.5f;
        return position + Vector3.up * halfSize;
    }

    private float GetMaxLossyRange()
    {
        float range = GetStretchType() switch { 
            StretchType.Horizontal => m_boxProximityField.transform.lossyScale.x,
            StretchType.Vertical => m_boxProximityField.transform.lossyScale.y,
            _ => 0.0f
        };

        return range;
    }

    private float GetMaxLocalRange()
    {
        float range = GetStretchType() switch { 
            StretchType.Horizontal => m_boxProximityField.transform.localScale.x,
            StretchType.Vertical => m_boxProximityField.transform.localScale.y,
            _ => 0.0f
        };

        return range;
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
