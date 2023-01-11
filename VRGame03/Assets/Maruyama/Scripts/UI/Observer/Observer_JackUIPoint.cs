using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UniRx;

public class Observer_JackUIPoint : MonoBehaviour
{
    public struct Data
    {
        public Jackable jakable;
        public JackPointUI ui;

        public Data(Jackable jackable, JackPointUI ui)
        {
            this.jakable = jackable;
            this.ui = ui;
        }
    }

    private List<Data> m_datas = new List<Data>();

    private List<JackPointUI> m_observeUIs = new List<JackPointUI>();

    [SerializeField]
    private Factory_Touch_JackUI m_factory;

    [SerializeField]
    private JackCameraUI m_cameraUI;               //ジャック先を表示するUI

    private JackPointUI m_currentPointUI = null;   //現在選択中のUI

    private void Start()
    {
        m_observeUIs = m_factory.GetJackPointUIs();

        foreach (var ui in m_observeUIs)
        {
            m_datas.Add(new Data(ui.GetJakable(), ui));

            //切り替わったときに呼びたい処理の登録
            ui.ObservableIsSelect
                .Skip(1)
                .Subscribe(value => ui.GetJakable().UISelectEvent(value))
                .AddTo(this);

            ui.AddSelectEvent(TouchEvent);
        }
    }

    private void TouchEvent(JackPointUI pointUI)
    {
        //同じならcurrentをnullに変えて処理を終了
        if(m_currentPointUI == pointUI) {
            m_currentPointUI = null;
            return;
        }

        m_currentPointUI.SetIsSelect(false);
        m_currentPointUI = pointUI; //現在選択中のUIを設定
    }
}
