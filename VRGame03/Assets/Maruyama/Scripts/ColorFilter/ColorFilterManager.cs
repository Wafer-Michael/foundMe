using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using maru.UtilityDictionary;

public class ColorFilterManager : SingletonMonoBehaviour<ColorFilterManager>
{
    //�l�C�r�[�u���[

    [System.Serializable]
    public enum ColorType {
        None,
        Red,
        Green,
        Blue,
        Purple,
        Cyan,

        Max,
    }

    [SerializeField]
    private Ex_Dictionary<ColorType, Color> m_colors;   //�J���[�}�b�v

    public Ex_Dictionary<ColorType, Color> Colors { get => m_colors; }  //�J���[�}�b�v�̃v���p�e�B

    [SerializeField]
    private ColorType m_currentColorType = ColorType.None;
    public ColorType CurrentColorType { get => m_currentColorType; }

    [SerializeField]
    private ColorFilter m_filter;

    private List<ColorFilterTargeter> m_colorControllers = new List<ColorFilterTargeter>();

    protected override void Awake()
    {
        base.Awake();

        m_colors.InsertInspectorData();     //�J���[�f�[�^�̑}��

        if (m_filter == null)
        {
            m_filter = FindObjectOfType<ColorFilter>();
        }

        //���ŃJ���[����̕K�v�ȃI�u�W�F�N�g��ǉ�
        m_colorControllers = new List<ColorFilterTargeter>(FindObjectsOfType<ColorFilterTargeter>());
    }

    private void Start()
    {
        ChangeNextColor();
    }

    private void Update()
    {
        if (!CanChangeColor())
        {
            return;
        }

        if (PlayerInputer.IsChangeColor())
        {
            ChangeNextColor();
        }
    }

    /// <summary>
    /// �J���[�̎擾
    /// </summary>
    /// <param name="type">�J���[�^�C�v</param>
    /// <returns></returns>
    public Color GetColor(ColorType type) { return m_colors[type]; }

    public void ChangeColor(ColorType type)
    {
        m_currentColorType = type;
        m_filter.ChangeColor(GetColor(type));

        if(CurrentColorType == ColorType.None) {    //�J���[�^�C�v��None�Ȃ瓧�ߏ��������Ȃ�
            return;
        }

        //�I�u�W�F�N�g�̓��ߕύX
        foreach(var colorController in m_colorControllers)
        {
            if(colorController.ColorType == CurrentColorType)
            {
                colorController.Hide();
            }
            else
            {
                colorController.Appear();
            }
        }
    }

    private void ChangeNextColor()
    {
        ChangeColor(GetNextColorType());
    }

    private ColorType GetNextColorType()
    {
        int colorIndex = (int)CurrentColorType;
        colorIndex++;

        if (colorIndex >= (int)ColorType.Max)
        {
            colorIndex = 0;
        }

        return (ColorType)colorIndex;
    }

    public bool CanChangeColor()
    {
        return m_colors.Count != 0;
    }
}
