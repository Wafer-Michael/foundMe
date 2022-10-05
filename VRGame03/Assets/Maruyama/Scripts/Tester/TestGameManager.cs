using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestGameManager : MonoBehaviour
{
    [System.Serializable]
    struct StartData {
        public GameObject prefab;
        public Vector3 startPosition;

        public StartData(Vector3 position)
        {
            prefab = null;
            startPosition = position;
        }
    }

    [SerializeField]
    StartData m_startData = new StartData(new Vector3(0.0f, 1.5f, 0.0f));

    void Start()
    {
        if (m_startData.prefab)
        {
            m_startData.prefab.transform.position = m_startData.startPosition;
        }
    }

    void Update()
    {
        
    }
}
