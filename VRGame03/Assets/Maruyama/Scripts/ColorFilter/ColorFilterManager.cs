using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using maru.UtilityDictionary;

public class ColorFilterManager : SingletonMonoBehaviour<ColorFilterManager>
{
    //ネイビーブルー

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
    private Ex_Dictionary<ColorType, Color> m_colors;   //カラーマップ

    public Ex_Dictionary<ColorType, Color> Colors { get => m_colors; }  //カラーマップのプロパティ

    [SerializeField]
    private ColorType m_currentColorType = ColorType.None;
    public ColorType CurrentColorType { get => m_currentColorType; }

    [SerializeField]
    private ColorFilter m_filter;

    private List<ColorFilterTargeter> m_colorControllers = new List<ColorFilterTargeter>();

    protected override void Awake()
    {
        base.Awake();

        m_colors.InsertInspectorData();     //カラーデータの挿入

        if (m_filter == null)
        {
            m_filter = FindObjectOfType<ColorFilter>();
        }

        //仮でカラー制御の必要なオブジェクトを追加
        m_colorControllers = new List<ColorFilterTargeter>(FindObjectsOfType<ColorFilterTargeter>());
    }

    private void Start()
    {
        if(m_filter == null)
        {
            return;
        }

        ChangeNextColor();
    }

    private void Update()
    {
        if (m_filter == null)
        {
            return;
        }

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
    /// カラーの取得
    /// </summary>
    /// <param name="type">カラータイプ</param>
    /// <returns></returns>
    public Color GetColor(ColorType type) { return m_colors[type]; }

    public void ChangeColor(ColorType type)
    {
        m_currentColorType = type;
        m_filter.ChangeColor(GetColor(type));

        if(CurrentColorType == ColorType.None) {    //カラータイプがNoneなら透過処理を入れない
            return;
        }

        //オブジェクトの透過変更
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
