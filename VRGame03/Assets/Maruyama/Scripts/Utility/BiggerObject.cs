using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BiggerObject : MonoBehaviour
{
    [SerializeField]
    float m_smallSize = 0.0f;

    [SerializeField]
    float m_bigSize = 1.0f;

    [SerializeField]
    float m_speed = 1.0f;

    void Start()
    {
        var scale = transform.localScale * m_smallSize;
        transform.localScale = scale;
    }

    // Update is called once per frame
    void Update()
    {
        if(m_bigSize <= transform.localScale.x)
        {
            transform.localScale = Vector3.one * m_bigSize;
        }

        transform.localScale += Vector3.one * m_speed * Time.deltaTime;
    }
}
