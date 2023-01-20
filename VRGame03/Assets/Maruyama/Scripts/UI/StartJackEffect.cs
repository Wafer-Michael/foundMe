using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEngine.Events;

public class StartJackEffect : MonoBehaviour
{
    [System.Serializable]
    public struct Parametor
    {
        public float time;                  //最大スケールになるまでの時間
        public float scaleFactor;           //スケールの倍率
        public UnityEvent m_endFunction;    //終了時に呼び出したい処理
    }

    [SerializeField]
    private Parametor m_param = new Parametor() { 
        time = 10.0f,
        scaleFactor = 1.5f
    };

    private Vector3 m_initializeScale;  //初期スケール

    private System.Action m_updateFunction;

    private void Awake()
    {
        m_initializeScale = transform.localScale;
        m_updateFunction = null;
    }

    private void Update()
    {
        m_updateFunction?.Invoke();
    }

    private void UpdateProcess()
    {
        ScaleUpdate();
        MoveUpdate();

        if (IsEnd()) {
            m_updateFunction = null;
        }
    }

    private void ScaleUpdate()
    {
        var targetScale = m_initializeScale * m_param.scaleFactor;
        var subScale = targetScale - transform.localScale;

        var scale = transform.localScale;
        var resultScale = scale + (subScale * GetFadeSpeed());

        transform.localScale = resultScale;

        if (IsEnd()) {
            transform.localScale = targetScale;
        }
    }

    private void MoveUpdate()
    {

    }

    public void StartFade()
    {
        m_updateFunction = UpdateProcess;
    }

    private float GetFadeSpeed() { return m_param.scaleFactor / m_param.time; }

    public bool IsEnd() { 
        var targetScale = m_initializeScale * m_param.scaleFactor;
        return transform.localScale.x >= targetScale.x;
    }

}
