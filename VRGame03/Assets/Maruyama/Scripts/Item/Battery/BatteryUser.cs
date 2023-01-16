using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BatteryUser : MonoBehaviour
{
    public static readonly Parametor DEFAULT_PARAMETOR = new Parametor() {
        useTime = 20
    };

    [System.Serializable]
    public struct Parametor
    {
        public float useTime;   //�g�p����
    }

    [SerializeField]
    private Parametor m_param = DEFAULT_PARAMETOR;      //�p�����[�^

    private Battery m_battery = null;                   //�o�b�e���[

    private GameTimer m_timer = new GameTimer();

    protected void Awake()
    {
        Charge(new Battery());
    }

    protected void Update()
    {
        if (m_timer.IsTimeUp) {
            return;
        }

        m_timer.UpdateTimer();

        m_battery.SetValue(m_battery.MaxValue * m_timer.IntervalTimeRate);
    }

    /// <summary>
    /// �d�r�`���[�W
    /// </summary>
    /// <param name="battrey">�`���[�W����o�b�e���[</param>
    public void Charge(Battery battery) {
        if (m_battery) {    //�o�b�e���[�����݂���Ȃ�
            Destroy(m_battery.gameObject);     //���݂̃o�b�e���[���폜
        }        

        m_battery = battery;    //�o�b�e���[�̌���
        m_timer.ResetTimer(m_param.useTime * battery.GetBatteryRate()); //���Ԍv��
    }


    public float GetBatteryRate() { return m_battery.GetBatteryRate(); }
}
