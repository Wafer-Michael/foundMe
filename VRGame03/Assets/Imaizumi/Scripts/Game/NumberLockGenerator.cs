using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using UnityEngine;
using UnityEngine.UI;

public class NumberLockGenerator : MonoBehaviour
{
    public enum NumberType
    {
        WallPattern,
        WallColor,
        DoorColor
    }

    enum DoorType
    {
        Single,
        Double
    }

    private static System.Random sm_random = new System.Random(System.DateTime.Now.Millisecond);

    [SerializeField]
    List<string> m_wallPatterns = new List<string>();
    Dictionary<string, int> m_keyWallPattern = new Dictionary<string, int>();

    [SerializeField]
    List<string> m_wallColors = new List<string>();
    Dictionary<string, int> m_keyWallColor = new Dictionary<string, int>();

    [SerializeField]
    List<string> m_doorTextures = new List<string>();
    Dictionary<string, int> m_keyDoorColor = new Dictionary<string, int>();


    void Awake()
    {
        MakeKeyPattern(ref m_keyWallPattern, in m_wallPatterns);
        MakeKeyPattern(ref m_keyWallColor, in m_wallColors);
        MakeKeyPattern(ref m_keyDoorColor, in m_doorTextures);
    }

    void MakeKeyPattern(ref Dictionary<string, int> numberList,in List<string> attributeList)
    {
        List<int> choicesNums = new List<int>() { 0, 1, 2, 3, 4, 5, 6, 7, 8, 9 };

        foreach (var attri in attributeList)
        {
            int number =  sm_random.Next(0, choicesNums.Count);

            numberList.Add(attri, choicesNums[number]);

            choicesNums.RemoveAt(number);
        }
    }

    public int FetchNumber(Texture tex, NumberType type)
    {
        int result = 0;

        var texname = tex.name.ToString();

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

    int RegularExpression(Dictionary<string, int> dic, string textureName)
    {
        int result = 0;

        foreach(var key in dic.Keys)
        {
            if(Regex.IsMatch(textureName, key, RegexOptions.IgnoreCase))
            {
                result = dic[key];
            }
        }

        return result;
    }

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
