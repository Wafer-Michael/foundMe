using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ColorFilterTargeter : MonoBehaviour
{
    [SerializeField]
    private ColorFilterManager.ColorType m_colorType;   //�J���[�^�C�v
    public ColorFilterManager.ColorType ColorType { get => m_colorType; }

    [SerializeField]
    private Material m_material;            //�ύX�������}�e���A��(�ύX�������ꍇ�̂ݐݒ�)

    private Renderer m_renderer = null;     //�����_�[

    private void Awake()
    {
        m_renderer = GetComponent<Renderer>();
    }

    private void Start()
    {
        ChangeColor();
    }

    /// <summary>
    /// �J���[�̕ύX
    /// </summary>
    void ChangeColor()
    {
        if(m_colorType == ColorFilterManager.ColorType.None) {  //�J���[�^�C�v��None�Ȃ珈�����΂��B
            return;
        }

        if (m_material) //�}�e���A����ʂŐݒ肷��Ȃ�
        {
            m_renderer.material = m_material;
        }

        if (m_renderer)
        {
            m_renderer.material.color = ColorFilterManager.Instance.GetColor(m_colorType);
        }
    }

    /// <summary>
    /// �B���
    /// </summary>
    public void Hide()
    {
        foreach (Renderer children in GetComponentsInChildren<Renderer>())
        {
            children.enabled = false;
        }
    }

    /// <summary>
    /// �����
    /// </summary>
    public void Appear()
    {
        foreach (Renderer children in GetComponentsInChildren<Renderer>())
        {
            children.enabled = true;
        }
    }
}
