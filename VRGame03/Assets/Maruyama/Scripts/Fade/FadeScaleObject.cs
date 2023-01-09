using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEngine.Events;

public class FadeScaleObject : FadeObject
{
    [SerializeField]
    private float m_maxRange = 1.0f;
    public void SetMaxRange(float range) { m_maxRange = range; }

    [SerializeField]
    private GameObject m_fadeObject;

    [SerializeField]
    private float m_fadeTime = 0.25f;

    [SerializeField]
    private bool m_playOnAwake = false;

    [SerializeField]
    private FadeType m_fadeType = FadeType.FadeOut;

    private bool m_isFading = false;

    private bool m_isFinish = false;

    [SerializeField]
    private UnityEvent m_finishEvent;

    public override void FadeStart()
    {
        if (!m_isFading)
        {
            StartCoroutine(Fading(m_fadeTime));
        }
    }

    public override void FadeStart(FadeType type)
    {
        if (!m_isFading)
        {
            m_fadeType = type;
            StartCoroutine(Fading(m_fadeTime));
        }
    }

    public override bool IsFinish() => m_isFinish;

    private IEnumerator Fading(float fadeTime)
    {
        float countTime = 0.0f;

        m_isFading = true;

        var scale = m_fadeObject.transform.localScale;

        while (countTime < fadeTime)
        {
            countTime += Time.unscaledDeltaTime;

            float ratio = countTime / fadeTime;

            if (m_fadeType == FadeType.FadeIn)
            {
                ratio = 1.0f - ratio;
            }

            m_fadeObject.transform.localScale = new Vector3(ratio * m_maxRange, scale.y, scale.z);

            yield return null;
        }

        float finishRatio = m_fadeType == FadeType.FadeOut ? 1.0f : 0.0f;

        m_fadeObject.transform.localScale = new Vector3(finishRatio * m_maxRange, scale.y, scale.z);        

        m_isFinish = true;

        m_isFading = false;

        Debug.Log("ššš");
        m_finishEvent.Invoke();
    }

    private void Reset()
    {
        //if (!m_)
        //{
        //    m_stretchController = GetComponentInParent<UIStretchController>();
        //}
    }

    private void Awake()
    {
        if (m_playOnAwake)
        {
            FadeStart();
        }
    }
}
