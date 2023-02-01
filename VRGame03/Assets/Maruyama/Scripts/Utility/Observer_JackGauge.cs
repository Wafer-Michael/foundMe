using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UniRx;

public class Observer_JackGauge : MonoBehaviour
{
    [SerializeField]
    private JackController m_jackController;    //�W���b�N�Ǘ�

    [SerializeField]
    private FillAmoutGauge m_gaugeUI;           //�Q�[�WUI

    private void Awake()
    {
        //�W���b�N�R���g���[���[�̏�ԕω��ɂ���āA�ύX����B
        SettingJackObserve();
    }

    private void Update()
    {
        //�Q�[�W�̃I�u�W�F�N�g�������Ă����Ԃ�������B
        if (m_gaugeUI.gameObject.activeSelf) {
            m_gaugeUI.FillAmoutValue = m_jackController.GetIntervalTimeRate();
        }
    }

    /// <summary>
    /// �W���b�N�R���g���[���[�̊Ď��ݒ�
    /// </summary>
    private void SettingJackObserve()
    {
        m_jackController.IsJackObserver
            .Subscribe(value => ChangeActiveGauge(value))
            .AddTo(this);
    }

    /// <summary>
    /// �Q�[�W�̃A�N�e�B�u��Ԃ�ύX
    /// </summary>
    private void ChangeActiveGauge(bool isActive)
    {
        m_gaugeUI.gameObject.SetActive(isActive);
    }
}
