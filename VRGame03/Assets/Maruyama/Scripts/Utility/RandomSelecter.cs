using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RandomSelecter : MonoBehaviour
{
    private List<GameObject> m_targets = new List<GameObject>();

    private void Awake()
    {
        for(int i = 0; i < transform.childCount; ++i)
        {
            m_targets.Add(transform.GetChild(i).gameObject);
        }

        var target = MaruUtility.MyRandom.RandomList(m_targets);
        target.SetActive(true);
    }
}
