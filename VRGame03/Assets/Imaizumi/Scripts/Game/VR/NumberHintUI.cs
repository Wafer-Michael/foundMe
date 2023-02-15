using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class NumberHintUI : MonoBehaviour
{
    [SerializeField]
    GameObject m_generator;

    [SerializeField]
    NumberLockGenerator.NumberType m_numberType;

    [SerializeField]
    List<Sprite> m_Sprites = new List<Sprite>();

    Dictionary<string, int> m_dictionary = new Dictionary<string, int>();

    // Start is called before the first frame update
    void Start()
    {
        var geneComp = m_generator.GetComponent<NumberLockGenerator>();
        m_dictionary = geneComp.GetDirectionary(m_numberType);

        DisplayHint();
    }

    void DisplayHint()
    {
        int index = 0;
        var keys = m_dictionary.Keys;
        foreach (var key in keys)
        {
            var childText = this.gameObject.transform.GetChild(index).gameObject.GetComponent<Text>();

            var num = m_dictionary[key].ToString();
            childText.text =  num;

            index++;
        }
    }
}