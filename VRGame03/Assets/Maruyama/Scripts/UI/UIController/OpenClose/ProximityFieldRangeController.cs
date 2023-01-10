using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Oculus.Interaction;

public class ProximityFieldRangeController : MonoBehaviour
{
    public enum PivotType { 
        Left,   //��
        Right,  //�E
    }

    [SerializeField]
    private UIStretchController m_stretchController;    //�X�g���b�`�R���g���[���[

    [SerializeField]
    private UIStretchRangeEvent m_stretchRangeEvent;    //�X�g���b�`�����W�C�x���g

    [SerializeField]
    private BoxProximityField m_proximityField;         //�{�b�N�X�t�B�[���h

    private PivotType m_pivotType = PivotType.Left;     //�s�{�b�g�|�C���g

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
        //float maxRange = m_stretchRangeEvent.GetRatioRange() * m_stretchController.GetMaxLossyRange();    //�X�g���b�`�����W�̍ő�l���擾

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
    /// �A�N�Z�b�T
    //--------------------------------------------------------------------------------------



}
