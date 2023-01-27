using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DoorLockUI : MonoBehaviour
{
    int m_digit = 0;

    List<Text> m_texts = new List<Text>();

    [SerializeField]
    GameObject m_resultText;

    [SerializeField]
    GameObject m_select;

    void Start()
    {
        foreach (var child in GetComponentsInChildren<Text>())
        {
            m_texts.Add(child);
            child.text = "0";
        }
    }

    void Update()
    {
        ChangeDigit();
        NumberShift();
    }

    void ChangeDigit()
    {
        // ���I��
        if (Input.GetKeyDown(KeyCode.LeftArrow))
        {
            m_digit -= 1;
            if (m_digit < 0)
            {
                m_digit = 0;
            }
            MoveSelect();
        }
        if (Input.GetKeyDown(KeyCode.RightArrow))
        {
            m_digit += 1;
            if (m_digit >= m_texts.Count)
            {
                m_digit = m_texts.Count - 1;
            }
            MoveSelect();
        }
    }

    void NumberShift()
    {
        // ��������
        int text = int.Parse(m_texts[(int)m_digit].text);

        if (Input.GetKeyDown(KeyCode.UpArrow))
        {

            text += 1; // �����𑝂₷

            // �������ő�l�𒴂�����
            if (text > 9)
            {
                text = 0; // 0�ɖ߂�
            }
        }
        if (Input.GetKeyDown(KeyCode.DownArrow))
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

    void MoveSelect()
    {
        var pos = m_select.GetComponent<RectTransform>();
        pos.anchoredPosition = new Vector2((m_digit - 1) * 70.0f, pos.anchoredPosition.y);
    }

    public void DisplayResult(int correct, int almost)
    {
        var text = m_resultText.GetComponent<Text>();

        text.text = "��v " + correct + " ��������v " + almost + " �s��v " + (m_texts.Count - correct - almost) + "\n" + text.text;
    }

    public void SetActiveUI(bool value)
    {
        var numChild = this.gameObject.transform.parent.childCount;
        for(int i = 0; i < numChild; i++)
        {
            this.gameObject.transform.parent.GetChild(i).gameObject.SetActive(value);
        }
    }

    public void ClearText()
    {
        foreach(var text in m_texts)
        {
            text.text = "0";
        }
        m_resultText.GetComponent<Text>().text = "";
    }
}
