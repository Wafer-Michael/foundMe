using System.Collections;
using System.Collections.Generic;
using UnityEngine;


/// <summary>
/// 乗っ取り制御
/// </summary>
public class HijackController : MonoBehaviour
{
    [System.Serializable]
    public class ParentParingData
    {
        private GameObject parentObject;
        public GameObject ParentObject { get => parentObject; set => parentObject = value; }
        public GameObject selfObject;
    }

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
    private GameObject m_modelObject;       //モデルのオブジェクト

    [SerializeField]
    private GameObject m_modelParentObject; //モデルの親オブジェクト

    [SerializeField]
    private List<ParentParingData> m_parentParingDatas = new List<ParentParingData>();

    [SerializeField]
    Parametor m_param;  //パラメータ
    public Parametor Param {
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
    public bool IsJack {
        private set => m_isJack.Value = value; 
        get => m_isJack.Value;
    }

    [SerializeField]
    private GameObject DebugHiJackGameObject;

    private void Awake()
    {
        m_timer = new GameTimer(0.0f);    
    }

    private void Start()
    {
        m_timer.ResetTimer(m_param.time);
        m_camBackData.position = transform.position;
    }

    private void Update()
    {
        if (PlayerInputer.IsDebugKeyDown(KeyCode.K))
        {
            StartHijack(DebugHiJackGameObject);
        }

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
        if (!IsJack) {  //ジャック状態ならやる必要がない。
            return;
        }

        transform.position = m_camBackData.position;
        transform.forward = m_camBackData.forward;

        foreach(var data in m_parentParingDatas)
        {
            if(data.ParentObject == null) {
                continue;
            }

            data.selfObject.transform.parent = data.ParentObject.transform;
        }

        IsJack = false;
    }

    /// <summary>
    /// ハイジャック開始
    /// </summary>
    /// <param name="target">ハイジャックターゲット</param>
    public void StartHijack(GameObject target)
    {
        //Jack中なら処理を飛ばす。
        if (IsJack) {   
            return;
        }

        SaveCamBackData();
        
        foreach(var data in m_parentParingDatas)
        {
            data.ParentObject = data.selfObject.transform.parent.gameObject;
            data.selfObject.transform.parent = null;
        }
        Warp(target);

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

    private void Warp(GameObject target)
    {
        transform.position = target.transform.position;
        transform.forward = target.transform.forward;
    }
}
