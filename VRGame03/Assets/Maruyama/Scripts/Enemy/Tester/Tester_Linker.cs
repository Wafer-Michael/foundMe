using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tester_Linker : MonoBehaviour
{

    [SerializeField]
    private GameObject m_linkerObject;

    private Vector3 m_offset;

    private void Start()
    {
        m_offset = transform.position - m_linkerObject.transform.position;
    }

    private void Update()
    {
        transform.position = m_linkerObject.transform.position + m_offset;
        transform.forward = m_linkerObject.transform.forward;
    }

}
