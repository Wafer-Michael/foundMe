using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// �h�A����UI����
/// </summary>
public class DoorLockUI : MonoBehaviour
{
    int m_digit = 0; // �I�𒆂̌���

    List<Text> m_texts = new List<Text>(); // ������\������Text

    [SerializeField]
    GameObject m_resultText; // �t�B�[�h�o�b�N��\������Text

    [SerializeField]
    GameObject m_select; // �I�𒆂̌��������\������

    void Start()
    {
        ClearText();        
    }

    void Update()
    {
        ChangeDigit();
        NumberShift();
    }

    /// <summary>
    /// ����I������
    /// </summary>
    void ChangeDigit()
    {
        // ���I��
        if (Input.GetKeyDown(KeyCode.LeftArrow)) // �����������ꍇ
        {
            m_digit -= 1;
            // �ړ�����
            if (m_digit < 0)
            {
                m_digit = 0;
            }
            MoveSelect();
        }
        if (Input.GetKeyDown(KeyCode.RightArrow)) // �����������ꍇ
        {
            m_digit += 1;
            // �ړ�����
            if (m_digit >= m_texts.Count)
            {
                m_digit = m_texts.Count - 1;
            }
            MoveSelect();
        }
    }

    /// <summary>
    ///  ������I������
    /// </summary>
    void NumberShift()
    {
        // ��������
        int text = int.Parse(m_texts[(int)m_digit].text);

        if (Input.GetKeyDown(KeyCode.UpArrow)) // �����������ꍇ
        {

            text += 1; // �����𑝂₷

            // �������ő�l�𒴂�����
            if (text > 9)
            {
                text = 0; // 0�ɖ߂�
            }
        }
        if (Input.GetKeyDown(KeyCode.DownArrow)) // �����������ꍇ
        {

            text -= 1; // ���������炷

            // �������ŏ��l�𒴂�����
            if (text < 0)
            {
                text = 9; // �ő�l�ɂ���
            }
        }

        m_texts[(int)m_digit].text = text.ToString(); // �\��
    }

    /// <summary>
    /// �I�𒆂̌�����������
    /// </summary>
    void MoveSelect()
    {
        var pos = m_select.GetComponent<RectTransform>();
        pos.anchoredPosition = new Vector2((m_digit - 1) * 70.0f, pos.anchoredPosition.y);
    }

    /// <summary>
    /// ���ʂ�\������
    /// </summary>
    /// <param name="correct">������</param>
    /// <param name="almost">�ɂ���������</param>
    public void DisplayResult(int correct, int almost)
    {
        var text = m_resultText.GetComponent<Text>();

        text.text = "��v " + correct + " ��������v " + almost + " �s��v " + (m_texts.Count - correct - almost) + "\n" + text.text;
    }

    /// <summary>
    /// UI�̕\����Ԃ�ݒ�
    /// </summary>
    /// <param name="value">�\�����</param>
    public void SetActiveUI(bool value)
    {
        var numChild = this.gameObject.transform.parent.childCount;
        for(int i = 0; i < numChild; i++)
        {
            this.gameObject.transform.parent.GetChild(i).gameObject.SetActive(value);
        }
    }

    /// <summary>
    /// �e�L�X�g�����Z�b�g
    /// </summary>
    public void ClearText()
    {
        foreach(var text in m_texts)
        {
            text.text = "0";
        }
        m_resultText.GetComponent<Text>().text = "";
    }
}
