using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 乗っ取り制御
/// </summary>
public class HijackController : MonoBehaviour
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

    GameTimer m_timer;  //タイマー

    private bool m_isJack = false;  //ジャック中かどうか
    public bool IsJack {
        private set => m_isJack = value; 
        get => m_isJack;
    }

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

    /// <summary>
    /// ハイジャックをやめて元に戻る。
    /// </summary>
    private void CamBack()
    {
        transform.position = m_camBackData.position;
        transform.forward = m_camBackData.forward;
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
        Warp(target);

        m_timer.ResetTimer(m_param.time);    //タイマースタート
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
