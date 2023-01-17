using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// playerを見失った時の徘徊ステート
/// </summary>
public class LostPatrol : EnemyStateNodeBase<EnemyBase>
{
    public struct Parametor 
    {
        public float time;      //探索をする最大時間
    }

    private Parametor m_param;  //パラメータ

    private GameTimer m_timer;  //タイマー管理クラス

    public LostPatrol(EnemyBase owner) :
        base(owner)
    { }

    public LostPatrol(EnemyBase owner, Parametor parametor) :
        base(owner)
    {
        m_param = parametor;
        m_timer = new GameTimer();
    }

    public override void OnStart()
    {
        base.OnStart();

        //影響マップを元に、徘徊ルートを決める。
        
    }

    public override bool OnUpdate()
    {
        //常に影響マップを更新し続ける。

        
        m_timer.UpdateTimer();   //時間計測

        return IsEnd();          //一定時間たったら、探索を終了する。
    }

    private bool IsEnd() { return m_timer.IsTimeUp; }
}
