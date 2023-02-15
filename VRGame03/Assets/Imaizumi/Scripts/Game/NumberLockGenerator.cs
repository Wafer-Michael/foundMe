using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// �h�A�̈Ïؔԍ��𐶐��A�Ǘ�����
/// </summary>
public class NumberLockGenerator : MonoBehaviour
{
    // �ԍ��̎��
    public enum NumberType
    {
        WallPattern,
        WallColor,
        DoorColor
    }

    private static System.Random sm_random = new System.Random(System.DateTime.Now.Millisecond); // �����_���������@

    [SerializeField]
    List<string> m_wallPatterns = new List<string>(); // �ǎ�
    Dictionary<string, int> m_keyWallPattern = new Dictionary<string, int>(); // �ǎ��Ƃ���ɑΉ����鐔

    [SerializeField]
    List<string> m_wallColors = new List<string>(); // �ǂ̐F
    Dictionary<string, int> m_keyWallColor = new Dictionary<string, int>(); // �ǂ̐F������ɑΉ����鐔

    [SerializeField]
    List<string> m_doorTextures = new List<string>(); // �h�A�̐F
    Dictionary<string, int> m_keyDoorColor = new Dictionary<string, int>(); // �h�A�̐F�Ƃ���ɑΉ����鐔


    void Awake()
    {
        // �Ïؔԍ��𐶐�����
        MakeKeyPattern(ref m_keyWallPattern, in m_wallPatterns);
        MakeKeyPattern(ref m_keyWallColor, in m_wallColors);
        MakeKeyPattern(ref m_keyDoorColor, in m_doorTextures);
    }

    /// <summary>
    /// �ԍ��𐶐�����
    /// </summary>
    /// <param name="numberList">���͐�</param>
    /// <param name="attributeList">Key�Ƃ��Ďg��List</param>
    void MakeKeyPattern(ref Dictionary<string, int> numberList,in List<string> attributeList)
    {
        List<int> choicesNums = new List<int>() { 0, 1, 2, 3, 4, 5, 6, 7, 8, 9 }; // �c��̐�

        foreach (var attri in attributeList)
        {
            int number =  sm_random.Next(0, choicesNums.Count);

            numberList.Add(attri, choicesNums[number]); // �l�̓���

            choicesNums.RemoveAt(number); // �g�p���������O��
        }
    }

    /// <summary>
    /// �ԍ����擾����
    /// </summary>
    /// <param name="tex">�e�N�X�`����</param>
    /// <param name="type">�ԍ��̎��</param>
    /// <returns></returns>
    public int FetchNumber(Texture tex, NumberType type)
    {
        int result = 0;

        var texname = tex.name.ToString();

        // type���Ƃɐ����𔻒�
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
    /// �ԍ���T��
    /// </summary>
    /// <param name="dic"></param>
    /// <param name="textureName">�T�������e�N�X�`���̖��O</param>
    /// <returns></returns>
    int RegularExpression(Dictionary<string, int> dic, string textureName)
    {
        int result = 0;

        foreach(var key in dic.Keys)
        {
            if(Regex.IsMatch(textureName, key, RegexOptions.IgnoreCase)) // key�����݂��邩�𐳋K�\���Ŕ��肷��
            {
                result = dic[key]; // �������ꍇ�͔ԍ���Ԃ�
            }
        }

        return result;
    }

    /// <summary>
    /// Dictionary���擾
    /// </summary>
    /// <param name="type">�擾���������X�g�̎��</param>
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
