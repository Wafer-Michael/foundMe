using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Oculus.Interaction;

public class ProximityFieldRangeController : MonoBehaviour
{
    public enum PivotType { 
        Left,   //左
        Right,  //右
    }

    [SerializeField]
    private UIStretchController m_stretchController;    //ストレッチコントローラー

    [SerializeField]
    private UIStretchRangeEvent m_stretchRangeEvent;    //ストレッチレンジイベント

    [SerializeField]
    private BoxProximityField m_proximityField;         //ボックスフィールド

    private PivotType m_pivotType = PivotType.Left;     //ピボットポイント

    private void Awake()
    {
        if (!m_stretchRangeEvent) {
            m_stretchRangeEvent = GetComponent<UIStretchRangeEvent>();
        }
    }

    public void SettingField()
    {
        SettingPosition();
        SettingScale();
    }

    private void SettingPosition()
    {
        //float maxRange = m_stretchRangeEvent.GetRatioRange() * m_stretchController.GetMaxLossyRange();    //ストレッチレンジの最大値を取得

        //var leftPosition = m_stretchController.CalculateFieldLeftPosition();
        //var position = leftPosition + (Vector3.right * maxRange);

        //m_proximityField.transform.position = position;
    }

    private void SettingScale()
    {
        //float maxSize = m_stretchRangeEvent.GetRatioRange() * m_stretchController.GetMaxSize();

        //var scale = m_proximityField.transform.localScale;
        //m_proximityField.transform.localScale = new Vector3(maxSize, scale.y, scale.z);
    }

    //--------------------------------------------------------------------------------------
    /// アクセッサ
    //--------------------------------------------------------------------------------------



}
