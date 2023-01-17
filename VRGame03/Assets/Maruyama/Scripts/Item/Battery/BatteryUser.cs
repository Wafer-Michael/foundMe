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
        public float useTime;   //使用時間
    }

    [SerializeField]
    private Parametor m_param = DEFAULT_PARAMETOR;      //パラメータ

    private Battery m_battery = null;                   //バッテリー

    private GameTimer m_timer = new GameTimer();

    protected void Awake()
    {
        Charge(new Battery());
    }

    protected void Update()
    {
        //入力判定
        if (PlayerInputer.IsBatteryCharge()) {
            ChargeProcess();
        }

        if (m_timer.IsTimeUp) {
            return;
        }

        m_timer.UpdateTimer();

        m_battery.SetValue(m_battery.MaxValue * m_timer.IntervalTimeRate);
    }

    private void ChargeProcess()
    {
        var battery = GetComponentInParent<ItemBag>().TakeItem<Battery>();
        if (battery) {
            Charge(battery);
        }
    }

    /// <summary>
    /// 電池チャージ
    /// </summary>
    /// <param name="battrey">チャージするバッテリー</param>
    public void Charge(Battery battery) {
        if (m_battery) {    //バッテリーが存在するなら
            Destroy(m_battery.gameObject);     //現在のバッテリーを削除
        }        

        m_battery = battery;    //バッテリーの交換
        m_timer.ResetTimer(m_param.useTime * battery.GetBatteryRate()); //時間計測
    }

    /// <summary>
    /// バッテリーの残りレート
    /// </summary>
    /// <returns></returns>
    public float GetBatteryRate() { return m_battery.GetBatteryRate(); }
}
