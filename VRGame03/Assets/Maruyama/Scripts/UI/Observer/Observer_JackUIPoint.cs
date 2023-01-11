using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UniRx;

public class Observer_JackUIPoint : MonoBehaviour
{
    [SerializeField]
    private Factory_Touch_JackUI m_factory;             //UI生成クラス

    [SerializeField]
    private JackCameraUI m_cameraUI;                    //ジャック先を表示するUI

    private Selectable_VRUI m_currentPointUI = null;    //現在選択中のUI

    private void Start()
    {
        var pairDatas = m_factory.GetPariDatas();

        foreach (var data in pairDatas)
        {
            var ui = data.ui;
            var jakable = data.jakable;

            //切り替わったときに呼びたい処理の登録
            ui.ObservableIsSelect
                .Skip(1)
                .Subscribe(value => jakable.UISelectEvent(value))
                .AddTo(this);

            ui.AddSelectEvent(TouchEvent);  //タッチした時に呼び出したい処理
        }
    }

    private void TouchEvent(Selectable_VRUI pointUI)
    {
        if(m_currentPointUI == null) {
            m_currentPointUI = pointUI;
            return;
        }

        //同じならcurrentをnullに変えて処理を終了
        if(m_currentPointUI == pointUI) {
            m_currentPointUI = null;
            return;
        }

        m_currentPointUI.SetIsSelect(false);
        m_currentPointUI = pointUI; //現在選択中のUIを設定
    }
}
