using System.Collections;
using System.Collections.Generic;
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

    [SerializeField]
    List<Texture> m_wallTextures = new List<Texture>();
    Dictionary<Texture, int> m_keyWallPattern = new Dictionary<Texture, int>();
    Dictionary<Texture, int> m_keyWallColor = new Dictionary<Texture, int>();

    [SerializeField]
    List<Texture> m_doorTextures = new List<Texture>();
    Dictionary<Texture, int> m_keyDoorColor = new Dictionary<Texture, int>();


    void Start()
    {
        MakeKeyPattern(ref m_keyWallPattern, in m_wallTextures);
        MakeKeyPattern(ref m_keyWallColor, in m_wallTextures);
        MakeKeyPattern(ref m_keyDoorColor, in m_doorTextures);
    }

    void MakeKeyPattern(ref Dictionary<Texture, int> numberList,in List<Texture> textureList)
    {
        List<int> choicesNums = new List<int>() { 0, 1, 2, 3, 4, 5, 6, 7, 8, 9 };

        foreach (var texture in textureList)
        {
            int number = Random.Range(0, choicesNums.Count);

            numberList.Add(texture, choicesNums[number]);
            //Debug.Log("number" + texture + numberList[texture]);

            choicesNums.RemoveAt(number);
        }
    }

    public int SeekNumberList(Texture texName, NumberType type)
    {
        int result = 0;
        switch (type)
        {
            case NumberType.WallPattern:
                result = m_keyWallPattern[texName];
                break;

            case NumberType.WallColor:
                result = m_keyWallColor[texName];
                break;

            case NumberType.DoorColor:
                result = m_keyDoorColor[texName];
                break;
        }
        return result;
    }
}
