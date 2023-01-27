using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JackController : MonoBehaviour
{
    /// <summary>
    /// パラメータ
    /// </summary>
    [System.Serializable]
    public struct Parametor
    {
        public float time; //時間
    }

    /// <summary>
    /// 戻る用のデータ
    /// </summary>
    public struct CamBackData
    {
        public Vector3 position;
        public Vector3 forward;
    }

    [SerializeField]
    Parametor m_param;  //パラメータ
    public Parametor Param
    {
        get => m_param;
        set => m_param = value;
    }

    CamBackData m_camBackData;  //戻るデータ
    public CamBackData CamBackDataProperty
    {
        get => m_camBackData;
        set => m_camBackData = value;
    }

    private GameTimer m_timer;      //タイマー

    //ジャック中を表す。
    private UniRx.ReactiveProperty<bool> m_isJack = new UniRx.ReactiveProperty<bool>(false);
    public System.IObservable<bool> IsJackObserver => m_isJack;
    public bool IsJack
    {
        private set => m_isJack.Value = value;
        get => m_isJack.Value;
    }

    [SerializeField]
    private DissolveFadeSprite m_dissolveFadeSprite;

    [SerializeField]
    private GameObject m_returnTouchUI;

    private void Awake() {
        m_timer = new GameTimer(0.0f);
    }

    private void Start()
    {
        m_timer.ResetTimer(m_param.time);
        m_camBackData.position = transform.position;
    }

    private void Update()
    {
        if (IsJack)
        {
            m_timer.UpdateTimer();

            if (m_timer.IsTimeUp)   //タイムアップしたら、TimeOver処理
            {
                TimeOver();
            }
        }
    }

    /// <summary>
    /// 時間切れの時の処理
    /// </summary>
    private void TimeOver()
    {
        CamBack();
    }

    public void ForceCamBack()
    {
        CamBack();
    }

    /// <summary>
    /// ハイジャックをやめて元に戻る。
    /// </summary>
    private void CamBack()
    {
        if (!IsJack) {  //ジャック状態でないなら、やる必要がない。
            return;
        }

        //フェード開始
        UnityEngine.Events.UnityAction finishAction = () => m_dissolveFadeSprite.FadeStart(FadeObject.FadeType.FadeIn);
        finishAction += () => {
            transform.position = m_camBackData.position;
            transform.forward = m_camBackData.forward;
            IsJack = false;
        };

        m_dissolveFadeSprite.FadeStart(FadeObject.FadeType.FadeOut, finishAction);  //フェードスタート
        m_returnTouchUI.SetActive(false);
    }

    /// <summary>
    /// ハイジャック開始
    /// </summary>
    /// <param name="target">ハイジャックターゲット</param>
    public void StartHijack(Jackable target)
    {
        //Jack中なら処理を飛ばす。
        if (IsJack) {
            return;
        }

        if (target == null) {   //ターゲットのnullCheck
            return;
        }

        SaveCamBackData();      //戻る場所を記録する。

        Jack(target);

        m_timer.ResetTimer(m_param.time);       //タイマースタート
        IsJack = true;
    }

    /// <summary>
    /// 戻る用のデータを取得
    /// </summary>
    private void SaveCamBackData()
    {
        m_camBackData.position = transform.position;
        m_camBackData.forward = transform.forward;
    }

    private void Jack(Jackable target)
    {
        transform.position = target.transform.position + target.PositionOffset;
        transform.forward = target.transform.forward;

        m_returnTouchUI.SetActive(true);
    }
}
