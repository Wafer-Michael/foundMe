using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Oculus.Interaction;

public class StretchUIChildObject : MonoBehaviour
{
    [SerializeField]
    private UIStretchController m_parent;

    [SerializeField]
    private GameObject m_fadeObject;

    [SerializeField]
    private float m_maxSize = 0.8f;

    private Vector3 m_initializePosition;

    private FadeScaleObject m_fadeScaleComponent;

    private bool m_isUpdate = true;
    public bool IsUpdate() { return m_isUpdate; }
    public void SetIsUpdate(bool isUpdate) { m_isUpdate = isUpdate; }

    public void Awake()
    {
        m_fadeScaleComponent = GetComponent<FadeScaleObject>();

        m_initializePosition = m_fadeObject.transform.position;
    }

    public void Update()
    {
        if (IsUpdate())
        {
            UpdatePosition();
            UpdateScale();
        }
    }

    private void UpdatePosition()
    {
        var ratio = m_parent.GetCurrentRatio();         //割合の取得
        var maxOffsetRange = CalculateMaxOffsetRange(); //最大位置
        var offsetVec = m_initializePosition - m_parent.CalculateFieldLeftPosition();

        var range = maxOffsetRange * ratio;

        var position = m_parent.CalculateFieldLeftPosition() + (offsetVec.normalized * range);
        m_fadeObject.transform.position = position;
    }

    private void UpdateScale()
    {
        float ratio = m_parent.GetCurrentRatio();
        var scale = m_fadeObject.transform.localScale;
        m_fadeObject.transform.localScale = new Vector3(ratio * m_maxSize, scale.y, scale.z);
    }

    private float CalculateMaxOffsetRange()
    {
        var toInitializePositionVec = m_initializePosition - m_parent.CalculateFieldLeftPosition();
        return toInitializePositionVec.magnitude;
    }

    private void OnEnable()
    {
        m_fadeObject.transform.position = m_initializePosition;
        m_fadeScaleComponent?.SetMaxRange(m_maxSize);
        m_fadeScaleComponent?.FadeStart();
        m_isUpdate = false;
    }

    //--------------------------------------------------------------------------------------
    /// アクセッサ
    //--------------------------------------------------------------------------------------

    public void SetParent(UIStretchController parent) { m_parent = parent; }

    public UIStretchController GetParent() { return m_parent; }

}
