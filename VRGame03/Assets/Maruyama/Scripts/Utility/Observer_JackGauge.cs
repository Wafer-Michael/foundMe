using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UniRx;

public class Observer_JackGauge : MonoBehaviour
{
    [SerializeField]
    private JackController m_jackController;    //ジャック管理

    [SerializeField]
    private FillAmoutGauge m_gaugeUI;           //ゲージUI

    private void Awake()
    {
        //ジャックコントローラーの状態変化によって、変更する。
        SettingJackObserve();
    }

    private void Update()
    {
        //ゲージのオブジェクトが生きている状態だったら。
        if (m_gaugeUI.gameObject.activeSelf) {
            m_gaugeUI.FillAmoutValue = m_jackController.GetIntervalTimeRate();
        }
    }

    /// <summary>
    /// ジャックコントローラーの監視設定
    /// </summary>
    private void SettingJackObserve()
    {
        m_jackController.IsJackObserver
            .Subscribe(value => ChangeActiveGauge(value))
            .AddTo(this);
    }

    /// <summary>
    /// ゲージのアクティブ状態を変更
    /// </summary>
    private void ChangeActiveGauge(bool isActive)
    {
        m_gaugeUI.gameObject.SetActive(isActive);
    }
}
