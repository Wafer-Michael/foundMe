using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// ドア鍵のUI制御
/// </summary>
public class DoorLockUI : MonoBehaviour
{
    int m_digit = 0; // 選択中の桁数

    List<Text> m_texts = new List<Text>(); // 数字を表示するText

    [SerializeField]
    GameObject m_resultText; // フィードバックを表示するText

    [SerializeField]
    GameObject m_select; // 選択中の桁を強調表示する

    void Start()
    {
        // 子オブジェクトのTextコンポーネントを格納
        for(int i = 0; i < this.transform.childCount; i++)
        {
            var chlildObj = this.transform.GetChild(i).gameObject;
            m_texts.Add(chlildObj.GetComponent<Text>());
        }
        ClearText();        
    }

    void Update()
    {
        ChangeDigit();
        NumberShift();
    }

    /// <summary>
    /// 桁を選択する
    /// </summary>
    void ChangeDigit()
    {
        var input = PlayerInputer.CalculateMoveVector();
        // 桁選択
        if (input.x <= -0.19f) // ←を押した場合
        {
            m_digit -= 1;
            // 移動制限
            if (m_digit < 0)
            {
                m_digit = 0;
            }
            MoveSelect();
        }
        if (input.x >= 0.19f) // →を押した場合
        {
            m_digit += 1;
            // 移動制限
            if (m_digit >= m_texts.Count)
            {
                m_digit = m_texts.Count - 1;
            }
            MoveSelect();
        }
    }

    /// <summary>
    ///  数字を選択する
    /// </summary>
    void NumberShift()
    {
        // 数字送り
        int text = int.Parse(m_texts[(int)m_digit].text);
        var input = PlayerInputer.CalculateMoveVector();

        if (input.z >= 0.19f) // ↑を押した場合
        {

            text += 1; // 数字を増やす

            // 数字が最大値を超えたら
            if (text > 9)
            {
                text = 0; // 0に戻る
            }
        }
        if (input.z <= -0.19f) // ↓を押した場合
        {

            text -= 1; // 数字を減らす

            // 数字が最小値を超えたら
            if (text < 0)
            {
                text = 9; // 最大値にする
            }
        }

        m_texts[(int)m_digit].text = text.ToString(); // 表示
    }

    /// <summary>
    /// 選択中の桁を強調する
    /// </summary>
    void MoveSelect()
    {
        var pos = m_select.GetComponent<RectTransform>();
        pos.anchoredPosition = new Vector2((m_digit - 1) * 70.0f, pos.anchoredPosition.y);
    }

    /// <summary>
    /// 結果を表示する
    /// </summary>
    /// <param name="correct">正答数</param>
    /// <param name="almost">惜しかった数</param>
    public void DisplayResult(int correct, int almost)
    {
        var text = m_resultText.GetComponent<Text>();

        text.text = "一致 " + correct + " 数字が一致 " + almost + " 不一致 " + (m_texts.Count - correct - almost) + "\n" + text.text;
    }

    /// <summary>
    /// UIの表示状態を設定
    /// </summary>
    /// <param name="value">表示状態</param>
    public void SetActiveUI(bool value)
    {
        var numChild = this.gameObject.transform.parent.childCount;
        for(int i = 0; i < numChild; i++)
        {
            this.gameObject.transform.parent.GetChild(i).gameObject.SetActive(value);
        }
    }

    /// <summary>
    /// テキストをリセット
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
