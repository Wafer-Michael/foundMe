using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SenceController : MonoBehaviour
{
    public enum State
    { 
        Normal,
        Scene,
    }

    static string[] DEFAULT_NORMAL_LAYERS = new string[] { "L_Obstacle" };
    static string[] DEFAULT_SENCE_LAYERS = new string[] { "L_Obstacle" };

    [SerializeField]
    private Camera m_camera;

    [SerializeField]
    private string[] m_nomalEyeLayerStrings = DEFAULT_NORMAL_LAYERS;

    [SerializeField]
    private string[] m_senceEyeLayerStrings = DEFAULT_SENCE_LAYERS;

    private State m_state = State.Normal;
    public State CurrentState => m_state;

    [SerializeField]
    private float m_senceTime = 10.0f;

    [SerializeField]
    private float m_recastTime = 3.0f;

    private GameTimer m_timer = new GameTimer();
    private GameTimer m_recastTimer = new GameTimer();

    private void Update()
    {
        m_recastTimer.UpdateTimer();

        if (PlayerInputer.IsSence() && m_recastTimer.IsTimeUp) {
            StartSence();
        }

        if(CurrentState == State.Scene)
        {
            m_timer.UpdateTimer();
            if (m_timer.IsTimeUp)
            {
                EndSence();
            }
        }
    }

    private void StartSence()
    {
        //センス状態なら処理をしない。
        if(CurrentState == State.Scene) {
            return;
        }

        m_timer.ResetTimer(m_senceTime);
        m_state = State.Scene;

        var layer = LayerMask.GetMask(m_senceEyeLayerStrings);
        m_camera.cullingMask = layer;
    }

    private void EndSence()
    {
        m_state = State.Normal;

        var layer = LayerMask.GetMask(m_nomalEyeLayerStrings);
        m_camera.cullingMask = layer;

        m_recastTimer.ResetTimer(m_recastTime);
    }
}
