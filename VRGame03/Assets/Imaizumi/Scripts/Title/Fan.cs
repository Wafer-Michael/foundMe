using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fan : MonoBehaviour
{
    [SerializeField]
    GameObject[] m_fanObjs = new GameObject[2];

    [SerializeField]
    float m_rotationSpeed;

    void Update()
    {
        foreach(var fanObj in m_fanObjs)
        {
            var fanRota = fanObj.transform.rotation;
            fanObj.transform.Rotate(new Vector3(fanRota.x, m_rotationSpeed, fanRota.z) * Time.deltaTime);
        }
    }
}