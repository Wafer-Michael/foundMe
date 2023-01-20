using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// playerを見失った時の徘徊ステート
/// </summary>
public class LostPatrol : EnemyStateNodeBase<EnemyBase>
{
    private enum TaskEnum
    {
        
    }

    public struct Parametor 
    {
        public float time;                      //探索をする最大時間
        public AstarSeek.Parametor seekParam;   //追従パラメータ
    }

    private Parametor m_param;  //パラメータ

    private GameTimer m_timer;  //タイマー管理クラス

    private TargetManager m_targetManager;                          //ターゲット管理
    private AstarSeek m_astarSeek;                                  //AstarSeekの設定
    private SelfAstarNodeController m_selfAstarNodeController;      //自分自身が所属するAstarNodeの検索
    private SelfImpactCellController m_selfImpactCellController;    //自分が所属するセルを管理する。

    private TaskList<TaskEnum> m_taskList = new TaskList<TaskEnum>();

    public LostPatrol(EnemyBase owner) :
        this(owner, new Parametor() { time = 10.0f, seekParam = AstarSeek.DEFAULT_PARAMETOR })
    { }

    public LostPatrol(EnemyBase owner, Parametor parametor) :
        base(owner)
    {
        m_param = parametor;
        m_timer = new GameTimer();

        m_targetManager = owner.GetComponent<TargetManager>();
        m_astarSeek = owner.GetComponent<AstarSeek>();
        m_selfAstarNodeController = owner.GetComponent<SelfAstarNodeController>();
    }

    protected override void ReserveChangeComponents()
    {
        base.ReserveChangeComponents();

        var owner = GetOwner();
        AddChangeComp(owner, true, false);
    }

    public override void OnStart()
    {
        base.OnStart();

        //AstarSeekのパラメータを設定
        m_astarSeek.SetParametor(m_param.seekParam);

        StartAstar();   //Astarの開始
    }

    public override bool OnUpdate()
    {   
        m_timer.UpdateTimer();      //時間計測

        if (m_astarSeek.IsEnd()) {  //AstarSeekが終了したら
            StartAstar();
        }

        return IsEnd();     //一定時間たったら、探索を終了する。
    }

    private void StartAstar()
    {
        var factoryParametor = AIDirector.Instance.GetFieldWayPointsMap_FactoryParametor();
        var wayPointsMap = AIDirector.Instance.GetWayPointsMap().GetGraph();
        var selfNode = m_selfAstarNodeController.GetNode();
        var targetPosition = CalculateTargetPosition();

        //Astarの開始
        m_astarSeek.StartAstar(selfNode, targetPosition, wayPointsMap, factoryParametor.intervalRange);
    }

    private Vector3 CalculateTargetPosition()
    {
        var result = Vector3.zero;

        var wayPointsMap = AIDirector.Instance.GetWayPointsMap();   //ウェイポイントマップ
        var selfNode = m_selfAstarNodeController.GetNode();         //自分が所属する開始ノード

        //ロストした場所付近で危険度が高い(まだ確認を行っていない)場所を取得する。
        var openDatas = new Queue<AstarNode>();
        openDatas.Enqueue(selfNode);

        //openDataが空になるまで処理を続ける。
        while (openDatas.Count != 0) {
            //八方向のノードを取得する。

        }

        return result;
    }

    private bool IsEnd() { return m_timer.IsTimeUp; }
}
