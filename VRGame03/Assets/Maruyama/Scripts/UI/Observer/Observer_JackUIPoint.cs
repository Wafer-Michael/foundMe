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

    [SerializeField]
    private JackController m_jackController;            //ジャックコントローラー

    [SerializeField]
    private StartJackEffect m_jackEffect;               //ジャックエフェクト

    [SerializeField]
    private DissolveFadeSprite m_dissolveFadeSprite;    //ディゾブルフェード用のスプライト

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
            m_cameraUI.FadeStart(FadeObject.FadeType.FadeOut);
            return;
        }

        //同じならcurrentをnullに変えて処理を終了
        if(m_currentPointUI == pointUI) {
            Close();
            return;
        }

        m_currentPointUI.SetIsSelect(false);
        m_currentPointUI = pointUI; //現在選択中のUIを設定
    }

    /// <summary>
    /// 親UIが閉じた時にリセット処理
    /// </summary>
    public void Close()
    {
        if (IsClose()) {    //閉じているなら処理をしない
            return;
        }

        m_currentPointUI.SetIsSelect(false);
        m_currentPointUI = null;
        m_cameraUI.FadeStart(FadeObject.FadeType.FadeIn);
    }

    public bool IsClose() { return m_currentPointUI == null; }

    public void StartJackEffect()
    {
        //カレントUIがnullなら処理を飛ばす。
        if (m_currentPointUI == null) {
            return;
        }

        m_jackEffect.StartFade();
    }

    public void StartJack()
    {
        //カレントUIがnullなら処理を飛ばす。
        if(m_currentPointUI == null) {
            return;
        }

        UnityEngine.Events.UnityAction finishAction = () => m_dissolveFadeSprite.FadeStart(FadeObject.FadeType.FadeIn);
        finishAction += () => { m_jackController.StartHijack(FindSomeJakable(m_currentPointUI)); }; //ジャック開始
        finishAction += () => { Close(); };

        m_dissolveFadeSprite.FadeStart(FadeObject.FadeType.FadeOut, finishAction);         //フェード開始

        //m_jackController.StartHijack(FindSomeJakable(m_currentPointUI));   //ジャック開始
        //Close();
    }

    private Jackable FindSomeJakable(Selectable_VRUI ui)
    {
        foreach(var data in m_factory.GetPariDatas())
        {
            if(data.ui == ui) {
                return data.jakable;
            }
        }

        return null;
    }
}
