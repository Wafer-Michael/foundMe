using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// ドアの暗証番号を生成、管理する
/// </summary>
public class NumberLockGenerator : MonoBehaviour
{
    // 番号の種類
    public enum NumberType
    {
        WallPattern,
        WallColor,
        DoorColor
    }

    private static System.Random sm_random = new System.Random(System.DateTime.Now.Millisecond); // ランダム数生成機

    [SerializeField]
    List<string> m_wallPatterns = new List<string>(); // 壁紙
    Dictionary<string, int> m_keyWallPattern = new Dictionary<string, int>(); // 壁紙とそれに対応する数

    [SerializeField]
    List<string> m_wallColors = new List<string>(); // 壁の色
    Dictionary<string, int> m_keyWallColor = new Dictionary<string, int>(); // 壁の色をそれに対応する数

    [SerializeField]
    List<string> m_doorTextures = new List<string>(); // ドアの色
    Dictionary<string, int> m_keyDoorColor = new Dictionary<string, int>(); // ドアの色とそれに対応する数


    void Awake()
    {
        // 暗証番号を生成する
        MakeKeyPattern(ref m_keyWallPattern, in m_wallPatterns);
        MakeKeyPattern(ref m_keyWallColor, in m_wallColors);
        MakeKeyPattern(ref m_keyDoorColor, in m_doorTextures);
    }

    /// <summary>
    /// 番号を生成する
    /// </summary>
    /// <param name="numberList">入力先</param>
    /// <param name="attributeList">Keyとして使うList</param>
    void MakeKeyPattern(ref Dictionary<string, int> numberList,in List<string> attributeList)
    {
        List<int> choicesNums = new List<int>() { 0, 1, 2, 3, 4, 5, 6, 7, 8, 9 }; // 残りの数

        foreach (var attri in attributeList)
        {
            int number =  sm_random.Next(0, choicesNums.Count);

            numberList.Add(attri, choicesNums[number]); // 値の入力

            choicesNums.RemoveAt(number); // 使用した数を外す
        }
    }

    /// <summary>
    /// 番号を取得する
    /// </summary>
    /// <param name="tex">テクスチャ名</param>
    /// <param name="type">番号の種類</param>
    /// <returns></returns>
    public int FetchNumber(Texture tex, NumberType type)
    {
        int result = 0;

        var texname = tex.name.ToString();

        // typeごとに数字を判定
        switch (type)
        {
            case NumberType.WallPattern:
                result = RegularExpression(m_keyWallPattern, texname);
                break;

            case NumberType.WallColor:
                result = RegularExpression(m_keyWallColor, texname);
                break;

            case NumberType.DoorColor:
                result = RegularExpression(m_keyDoorColor, texname);
                break;
        }
        return result;
    }

    /// <summary>
    /// 番号を探す
    /// </summary>
    /// <param name="dic"></param>
    /// <param name="textureName">探したいテクスチャの名前</param>
    /// <returns></returns>
    int RegularExpression(Dictionary<string, int> dic, string textureName)
    {
        int result = 0;

        foreach(var key in dic.Keys)
        {
            if(Regex.IsMatch(textureName, key, RegexOptions.IgnoreCase)) // keyが存在するかを正規表現で判定する
            {
                result = dic[key]; // あった場合は番号を返す
            }
        }

        return result;
    }

    /// <summary>
    /// Dictionaryを取得
    /// </summary>
    /// <param name="type">取得したいリストの種類</param>
    /// <returns></returns>
    public Dictionary<string,int> GetDirectionary(NumberType type)
    {
        switch (type)
        {
            case NumberType.WallPattern:
                return m_keyWallPattern;

            case NumberType.WallColor:
                return m_keyWallColor;

            case NumberType.DoorColor:
                return m_keyDoorColor;
        }

        return null;
    }
}
