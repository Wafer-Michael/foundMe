using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAudioManager : MonoBehaviour
{
    [SerializeField]
    AudioSource m_sources;

    [SerializeField]
    float m_enemyStepSpan;
    float m_stepSpan = 0;

    VelocityManager m_velocity;

    private void Start()
    {
        var parent = this.gameObject.transform.parent.gameObject;
        m_velocity = parent.GetComponent<VelocityManager>();
    }

    void Update()
    {
        if (m_velocity?.velocity != Vector3.zero)
        {
            Debug.Log(1);
            if (m_stepSpan <= 0)
            {
                Debug.Log(2);

                m_sources.PlayOneShot(m_sources.clip);

                m_stepSpan = m_enemyStepSpan;
            }

            m_stepSpan -= Time.deltaTime;
        }
        else
        {
            m_stepSpan = 0;
        }
    }
}