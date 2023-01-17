using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class NumberLockGenerator : MonoBehaviour
{
    enum FlowerPattern
    {
        Large,
        Small,
        Plain
    }

    enum WallColor
    {
        Brown,
        Orange
    }

    enum DoorColor
    {
        Brown,
        white
    }

    enum DoorType
    {
        Single,
        Double
    }

    [SerializeField]
    List<Texture2D> m_flowers = new List<Texture2D>();
    Dictionary<Texture2D, int> m_keyFlowerPattern = new Dictionary<Texture2D, int>();

    [SerializeField]
    List<Texture2D> m_wallcolors = new List<Texture2D>();
    Dictionary<Texture2D, int> m_keyWallColor = new Dictionary<Texture2D, int>();

    [SerializeField]
    List<Texture2D> m_doorcolors = new List<Texture2D>();
    Dictionary<Texture2D, int> m_keyDoorColor = new Dictionary<Texture2D, int>();


    void Start()
    {
        MakeKeyPattern(ref m_keyFlowerPattern, in m_flowers);
        MakeKeyPattern(ref m_keyWallColor, in m_wallcolors);
        MakeKeyPattern(ref m_keyDoorColor, in m_doorcolors);
    }

    void MakeKeyPattern(ref Dictionary<Texture2D, int> numberList,in List<Texture2D> textureList)
    {
        foreach(var texture in textureList)
        {
            int number = Random.Range(0, 9);
            numberList.Add(texture, number);
            Debug.Log("number" + texture + numberList[texture]);
        }
    }
}
