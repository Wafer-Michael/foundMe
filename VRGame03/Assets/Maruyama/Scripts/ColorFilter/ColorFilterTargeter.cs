using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ColorFilterTargeter : MonoBehaviour
{
    [SerializeField]
    private ColorFilterManager.ColorType m_colorType;   //カラータイプ
    public ColorFilterManager.ColorType ColorType { get => m_colorType; }

    [SerializeField]
    private Material m_material;            //変更したいマテリアル(変更したい場合のみ設定)

    private Renderer m_renderer = null;     //レンダー

    private void Awake()
    {
        m_renderer = GetComponent<Renderer>();
    }

    private void Start()
    {
        ChangeColor();

        
    }

    /// <summary>
    /// カラーの変更
    /// </summary>
    void ChangeColor()
    {
        if(m_colorType == ColorFilterManager.ColorType.None) {  //カラータイプがNoneなら処理を飛ばす。
            return;
        }

        if (m_material) //マテリアルを別で設定するなら
        {
            m_renderer.material = m_material;
        }

        if (m_renderer)
        {
            m_renderer.material.color = ColorFilterManager.Instance.GetColor(m_colorType);
        }
    }

    /// <summary>
    /// 隠れる
    /// </summary>
    public void Hide()
    {
        foreach (Renderer children in GetComponentsInChildren<Renderer>())
        {
            children.enabled = false;
        }
    }

    /// <summary>
    /// 現れる
    /// </summary>
    public void Appear()
    {
        foreach (Renderer children in GetComponentsInChildren<Renderer>())
        {
            children.enabled = true;
        }
    }
}
