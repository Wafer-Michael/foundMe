using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEditor;

public class DoorLock : MonoBehaviour
{
    int m_digit = 3;

    List<int> m_lockNumbers = new List<int>();

    [SerializeField]
    bool m_isLock;
    public bool IsLock {get;}

    [SerializeField]
    GameObject m_numberText;

    [SerializeField]
    GameObject generator;

    private void Start()
    {
        m_digit = m_numberText.transform.childCount;
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.T))
        {
            AccessKey();
        }
    }

    /// <summary>
    /// 開錠前の処理
    /// </summary>
    void AccessKey()
    {
        DecisionDoorNumber();
        m_isLock = true;
        m_numberText.SetActive(true);
        m_numberText.GetComponent<DoorLockUI>().ResetNumber();
        StartCoroutine("Unlock");
        this.enabled = false;
    }

    /// <summary>
    /// 鍵を開ける
    /// </summary>
    IEnumerator Unlock()
    {
        Debug.Log("Start Coroutine");

        List<int> numbers = new List<int>();

        while (m_isLock)
        {
            yield return new WaitWhile(() => !Input.GetKeyDown(KeyCode.Space));
            numbers.Clear(); // 番号をリセット
            InputPass(ref numbers); // 番号入力
            StartCoroutine("Collation", numbers); // 照合

            yield break;
        }

        Debug.Log("unlocked");

        m_numberText.SetActive(false);
        this.enabled = true;
        yield break;
    }

    /// <summary>
    /// Listに番号を入力する
    /// </summary>
    /// <param name="numbers">入力するList</param>
    void InputPass(ref List<int> numbers)
    {
        for (int i = 0; i < m_digit; i++)
        {
            var numberText = m_numberText.transform.GetChild(i).gameObject;

            int textNum = int.Parse(numberText.GetComponent<Text>().text);
            numbers.Add(textNum);
        }

        Debug.Log("input number" + numbers[0] + numbers[1] + numbers[2]);
    }

    /// <summary>
    /// 入力した番号が正しいか判定する
    /// </summary>
    /// <param name="numbers">入力する番号</param>
    IEnumerator Collation(List<int> numbers)
    {
        int correct = 0;    //一致
        int almost = 0;     //惜しい

        for (int i = 0; i < numbers.Count; i++)
        {
            if (numbers[i] == m_lockNumbers[i])
            {
                correct += 1;
                numbers[i]  = -1;
            }
            yield return new WaitForEndOfFrame();
        }

        for (int i = 0; i < numbers.Count; i++)
        {
            for (int j = 0; j < m_lockNumbers.Count; j++)
            {
                if (numbers[i] == m_lockNumbers[j])
                {
                    almost++;
                    break;
                }
                yield return new WaitForEndOfFrame();
            }
        }

        if (correct == m_digit) // 一致した数が桁数と一緒なら
        {
            m_isLock = false;
            StartCoroutine("Unlock");
            yield break;
        }

        Debug.Log("一致 " + correct + "、 数字が一致 " + almost + "、 不一致 " + (m_digit - correct - almost));

        StartCoroutine("Unlock");
        yield break;
    }

    public void SetLockNumbers(List<int> numbers)
    {
        foreach(int num in numbers)
        {
            m_lockNumbers.Add(num);
        }
    }

    /// <summary>
    /// 暗証番号を取得する
    /// </summary>
    void DecisionDoorNumber()
    {
        var door = FindChildTag(this.gameObject, "Door");
        var doorTex = FetchTextureName(door); // ドアのテクスチャ

        var wall = FindChildTag(this.gameObject.transform.parent.gameObject, "Wall"); // 壁
        var wallTex = FetchTextureName(wall); // 壁のテクスチャ    

        List<int> numbers = new List<int>();

        // 番号を取得
        var numbergene = generator.GetComponent<NumberLockGenerator>();
        numbers.Add(numbergene.FetchNumber(wallTex, NumberLockGenerator.NumberType.WallPattern));
        numbers.Add(numbergene.FetchNumber(wallTex, NumberLockGenerator.NumberType.WallColor));
        numbers.Add(numbergene.FetchNumber(doorTex, NumberLockGenerator.NumberType.DoorColor));

        Debug.Log("lock Number " + numbers[0] + numbers[1] + numbers[2]);

        SetLockNumbers(numbers);
    }

    /// <summary>
    /// タグを持っている子オブジェクトを取得する
    /// </summary>
    /// <param name="parentObj">取得したいオブジェクトの親オブジェクト</param>
    /// <param name="tag">取得したいオブジェクトのタグ</param>
    /// <returns>子オブジェクト</returns>
    GameObject FindChildTag(GameObject parentObj, string tag)
    {
        GameObject result = null;

        var numChild = parentObj.transform.childCount; // 子オブジェクトの数
        for (int i = 0; i < numChild; i++)
        {
            var child = parentObj.transform.GetChild(i).gameObject;
            if (child.tag == tag)
            {
                result = child;
            }
        }

        return result;
    }

    /// <summary>
    /// オブジェクトが使用しているテクスチャを取得する
    /// </summary>
    /// <param name="gameObj">検索するオブジェクト</param>
    /// <returns>使用しているテクスチャ</returns>
    Texture FetchTextureName(GameObject gameObj)
    {
        Texture result = null; // 取得したテクスチャ

        var mat = gameObj.GetComponent<Renderer>().material; // オブジェクトのマテリアル
        var shader = mat.shader; // マテリアルが使用しているシェーダー

        var count = ShaderUtil.GetPropertyCount(shader);
        for (int i = 0; i < count; i++)
        {
            var type = ShaderUtil.GetPropertyType(shader, i);
            if (type == ShaderUtil.ShaderPropertyType.TexEnv)
            {
                var proName = ShaderUtil.GetPropertyName(shader, i);
                var tex = mat.GetTexture(proName);
                if (tex)
                {
                    result = tex;
                }
            }
        }

        return result;
    }
}