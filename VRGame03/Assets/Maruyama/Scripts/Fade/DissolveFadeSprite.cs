using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEngine.Events;

public class DissolveFadeSprite : FadeObject
{

    [SerializeField]
    private SpriteRenderer m_spriteRenderer;
    //private UIStretchController m_stretchController;

    private Material m_material;
    private int m_heightPropertyID;

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

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.D))
        {
            FadeStart();
        }
    }

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

        while (countTime < fadeTime)
        {
            countTime += Time.unscaledDeltaTime;

            float ratio = countTime / fadeTime;

            if (m_fadeType == FadeType.FadeIn)
            {
                ratio = 1.0f - ratio;
            }

            m_material.SetFloat(m_heightPropertyID, ratio);
            //m_stretchController.StretchUpdate(ratio);

            yield return null;
        }

        float finishRatio = m_fadeType == FadeType.FadeOut ? 1.0f : 0.0f;

        m_material.SetFloat(m_heightPropertyID, finishRatio);
        //m_stretchController.StretchUpdate(finishRatio);

        m_isFinish = true;

        m_isFading = false;

        m_finishEvent.Invoke();
    }

    private void Reset()
    {
        //if (!m_stretchController)
        {
            //m_stretchController = GetComponentInParent<UIStretchController>();
        }
    }

    private void Awake()
    {
        if(m_spriteRenderer == null) {
            m_spriteRenderer = GetComponent<SpriteRenderer>();
        }

        m_heightPropertyID = Shader.PropertyToID("_Height");
        m_material = m_spriteRenderer.material;

        if (m_playOnAwake)
        {
            FadeStart();
        }
    }
}
