using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DoorLockUI : MonoBehaviour
{
    int m_digit = 0;

    List<Text> m_texts = new List<Text>();

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

        }
        if (Input.GetKeyDown(KeyCode.RightArrow))
        {
            m_digit += 1;
            if (m_digit >= m_texts.Count)
            {
                m_digit = m_texts.Count - 1;
            }
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
}
