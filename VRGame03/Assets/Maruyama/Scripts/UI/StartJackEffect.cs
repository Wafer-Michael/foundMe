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
        public float speed;
        public float scaleFactor;           //スケールの倍率
        public UnityEvent m_endFunction;    //終了時に呼び出したい処理
    }

    [SerializeField]
    private Parametor m_param = new Parametor() { 
        time = 10.0f,
        speed = 0.1f,
        scaleFactor = 1.5f
    };

    private Vector3 m_initializeScale;  //初期スケール
    private Vector3 m_initializePosition;

    private System.Action m_updateFunction;

    private GameTimer m_timer = new GameTimer();

    private void Awake()
    {
        m_initializeScale = transform.localScale;
        m_initializePosition = transform.position;

        m_updateFunction = null;
    }

    private void Update()
    {
        m_updateFunction?.Invoke();
    }

    private void UpdateProcess()
    {
        //ScaleUpdate();
        //MoveUpdate();

        m_timer.UpdateTimer();

        //if (IsEnd()) {
        if (m_timer.IsTimeUp)
        {
            Debug.Log("★★★終了");

            m_param.m_endFunction?.Invoke();
            m_updateFunction = null;
        }
    }

    private void ScaleUpdate()
    {
        var targetScale = m_initializeScale * m_param.scaleFactor;
        var subScale = targetScale - transform.localScale;

        var scale = transform.localScale;
        var resultScale = scale + (targetScale * GetFadeSpeed() * Time.deltaTime);

        transform.localScale = resultScale;

        //Debug.Log("★LocalScale: " + transform.localScale.ToString());
        Debug.Log("★TargetScale: " + targetScale.ToString());
    }

    private void MoveUpdate()
    {
        var speed = m_param.speed / m_param.time;
        transform.position += transform.up * speed * Time.deltaTime;
    }

    public void StartFade()
    {
        m_updateFunction = UpdateProcess;
        m_timer.ResetTimer(m_param.time);
    }

    public void ReturnScale() {
        transform.localScale = m_initializeScale; 
        transform.position = m_initializePosition;
    }

    

    public float GetFadeSpeed() { return m_param.scaleFactor / m_param.time; }

    public bool IsEnd() { 
        var targetScale = m_initializeScale * m_param.scaleFactor;
        return transform.localScale.x >= targetScale.x;
    }

}
