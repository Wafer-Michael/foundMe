using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// ドアの暗証番号と鍵の状態を管理する
/// </summary>
public class DoorLock : MonoBehaviour
{
    int m_digit = 3; // 暗証番号の桁数

    List<int> m_lockNumbers = new List<int>(); // 暗証番号

    [SerializeField]
    bool m_isLock = true; // 鍵がかかっているかどうか
    public bool IsLock { get { return m_isLock; }}

    [SerializeField]
    GameObject m_canvas; // NumberTextの親オブジェクト
    GameObject m_numberText; // 生成したNumberText

    GameObject m_generator; // 番号生成機

    //int[] m_collationNumbers = new int[3];

    int m_correct = 0;    //一致
    int m_almost = 0;     //惜しい

    [SerializeField]
    int m_maxNumError; // 失敗できる最大数
    int m_numError; // 失敗した数

    System.Action m_action; // 開錠時のイベント
    System.Action m_errEvent; // エラー時のイベント

    [SerializeField]
    AudioSource m_unlockSE; // 開錠時のSE
    [SerializeField]
    AudioSource m_errSE; // エラー時のSE

    [SerializeField]
    GameObject m_front;
    [SerializeField]
    GameObject m_back;

    private void Awake()
    {
        m_generator = GameObject.Find("NumberLockGenerator");
    }

    private void Start()
    {
        var canvas = Instantiate(m_canvas); // キャンバス生成
        canvas.transform.parent = this.transform.parent; // キャンバスの親をRoomに設定

        // NumberTextを取得    
        for(int i = 0; i < canvas.transform.childCount; i++)
        {
            var child = canvas.transform.GetChild(i);
            var ui = child.GetComponent<DoorLockUI>();
            if(ui)
            {
                m_numberText = child.gameObject;
            }
        }

        m_digit = m_numberText.transform.childCount; // 桁数を更新
    }

    /// <summary>
    /// 開錠前の処理
    /// </summary>
    public void AccessKey(GameObject other)
    {
        if (!m_numberText.gameObject.activeInHierarchy)
        {
            DecisionDoorNumber();
            //ドアの場所を設定。
            float convart = ConvartDirection(other);
            
            //m_numberText.transform.parent.transform.position = transform.position + new Vector3(-0.625f, -2.75f, 0.15f);
            if(convart == 1) {
                m_numberText.transform.parent.transform.position = m_front.transform.position;
                m_numberText.transform.parent.transform.rotation = Quaternion.Euler(0.0f, 180.0f, 0.0f);
            }
            else {
                m_numberText.transform.parent.transform.position = m_back.transform.position;
                m_numberText.transform.parent.transform.rotation = Quaternion.Euler(0.0f, 0.0f, 0.0f);
            }
            
            m_numberText.GetComponent<DoorLockUI>().SetActiveUI(true);
            m_numberText.GetComponent<DoorLockUI>().ClearText();
            StartCoroutine("Unlock");
        }
    }

    private int ConvartDirection(GameObject other)
    {
        var requesterToOwner = transform.position - other.transform.position;
        float newDot = Vector3.Dot(requesterToOwner, transform.forward);

        return newDot > 0 ? -1 : 1;
    }



    /// <summary>
    /// アクセス中断
    /// </summary>
    public void Interruption()
    {
        m_numberText.GetComponent<DoorLockUI>().SetActiveUI(false);
        StopCoroutine("Unlock");
    }

    /// <summary>
    /// 鍵を開ける
    /// </summary>
    IEnumerator Unlock()
    {
        List<int> numbers = new List<int>();

        while (m_isLock) // 開錠されてなかったら
        {
            yield return new WaitWhile(() => !PlayerInputer.IsEnter()); // スペースが押されるまで待機
            // 各項目をリセット
            m_correct = 0;
            m_almost = 0;
            numbers.Clear();

            InputPass(ref numbers); // 番号入力
            StartCoroutine("Collation", numbers); // 照合

            yield break;
        }

        // 開錠時の処理
        m_action?.Invoke(); // イベント呼び出し
        m_unlockSE.PlayOneShot(m_unlockSE.clip);
        Interruption(); // アクセス中断

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
            var numberText = m_numberText.transform.GetChild(i).gameObject; // NumberTextを取得

            int textNum = int.Parse(numberText.GetComponent<Text>().text);
            numbers.Add(textNum); // 数字を追加
        }
    }

    /// <summary>
    /// 入力した番号が正しいか判定する
    /// </summary>
    /// <param name="numbers">入力する番号</param>
    IEnumerator Collation(List<int> numbers)
    {
        // 正答数を判定
        for (int i = 0; i < numbers.Count; i++)
        {
            if (numbers[i] == m_lockNumbers[i])
            {
                m_correct += 1;
                numbers[i]  = -1;
            }
            yield return new WaitForEndOfFrame();
        }

        // 惜しかった数を判定
        for (int i = 0; i < numbers.Count; i++)
        {
            for (int j = 0; j < m_lockNumbers.Count; j++)
            {
                if (numbers[i] == m_lockNumbers[j])
                {
                    m_almost++;
                    break;
                }
                yield return new WaitForEndOfFrame();
            }
        }

        if (m_correct == m_digit) // 一致した数が桁数と一緒なら
        {
            m_isLock = false; // 開錠
            StartCoroutine("Unlock");
            yield break;
        }

        // 以下開錠失敗時

        m_numberText.GetComponent<DoorLockUI>().DisplayResult(m_correct, m_almost); // フィードバック表示
        m_numError++; // エラー回数更新
        m_errSE.PlayOneShot(m_errSE.clip);
        if(m_numError >= m_maxNumError) // エラーが最大数に達したら
        {
            m_errEvent?.Invoke(); // イベント呼び出し
        }

        StartCoroutine("Unlock");
        yield break;
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
        var numbergene = m_generator.GetComponent<NumberLockGenerator>();
        numbers.Add(numbergene.FetchNumber(wallTex, NumberLockGenerator.NumberType.WallPattern));
        numbers.Add(numbergene.FetchNumber(wallTex, NumberLockGenerator.NumberType.WallColor));
        numbers.Add(numbergene.FetchNumber(doorTex, NumberLockGenerator.NumberType.DoorColor));

        Debug.Log("lock Number " + numbers[0] + numbers[1] + numbers[2]);

        SetLockNumbers(numbers);
    }

    /// <summary>
    /// 暗証番号を設定する
    /// </summary>
    /// <param name="numbers">暗証番号</param>
    public void SetLockNumbers(List<int> numbers)
    {
        foreach (int num in numbers)
        {
            m_lockNumbers.Add(num);
        }
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

        var count = shader.GetPropertyCount();
        for (int i = 0; i < count; i++)
        {
            var type = shader.GetPropertyType(i);
            if (type ==UnityEngine.Rendering.ShaderPropertyType.Texture)
            {
                var proName = shader.GetPropertyName(i);
                var tex = mat.GetTexture(proName);
                if (tex)
                {
                    result = tex;
                }
            }
        }

        return result;
    }

    /// <summary>
    /// 開錠時のイベント設定
    /// </summary>
    /// <param name="action">イベント</param>
    public void SetAction(System.Action action)
    {
        m_action = action;
    }

    /// <summary>
    /// エラー発生時のイベント設定
    /// </summary>
    /// <param name="action">イベント</param>
    public void SetErrorEvent(System.Action action)
    {
        m_errEvent = action;
    }
}