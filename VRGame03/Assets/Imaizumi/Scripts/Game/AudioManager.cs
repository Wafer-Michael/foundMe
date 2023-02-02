using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    [SerializeField]
    AudioSource m_sources;

    [SerializeField]
    float m_rightStepSpan;
    [SerializeField]
    float m_leftStepSpan;

    float m_stepSpan = 0;

    bool m_isRightStep = true;

    void Update()
    {
        if(PlayerInputer.CalculateMoveVector() != Vector3.zero)
        {
            Debug.Log(1);
            if (m_stepSpan <= 0)
            {
                Debug.Log(2);

                m_sources.PlayOneShot(m_sources.clip);

                if (m_isRightStep)
                {
                    m_stepSpan = m_leftStepSpan;
                }
                else
                {
                    m_stepSpan = m_rightStepSpan;
                }

                m_isRightStep = !m_isRightStep;
            }

            m_stepSpan -= Time.deltaTime;
        }
        else
        {
            m_stepSpan = 0;
        }
    }
}
