using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimerDestroy : MonoBehaviour
{
    [SerializeField]
    float time = 10.0f;

    GameTimer m_timer = new GameTimer(0.0f);

    private void Start()
    {
        m_timer.ResetTimer(time);
    }

    private void Update()
    {
        if (m_timer.IsTimeUp)
        {
            return;
        }

        m_timer.UpdateTimer();
        if (m_timer.IsTimeUp)
        {
            Destroy(gameObject);    
        }
    }
}
