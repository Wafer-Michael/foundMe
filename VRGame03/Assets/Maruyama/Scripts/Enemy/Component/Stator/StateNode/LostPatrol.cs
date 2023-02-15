using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using System.Linq;

namespace StateNode
{
    /// <summary>
    /// playerを見失った時の徘徊ステート
    /// </summary>
    public class LostPatrol : EnemyStateNodeBase<EnemyBase>
    {
        public struct Parametor
        {
            public float time;                      //探索をする最大時間
            public AstarSeek.Parametor seekParam;   //追従パラメータ
        }

        private Parametor m_param;  //パラメータ

        private GameTimer m_timer;  //タイマー管理クラス

        private AstarSeek m_astarSeek;                                  //AstarSeekの設定
        private SelfAstarNodeController m_selfAstarNodeController;      //自分自身が所属するAstarNodeの検索
        private SelfImpactCellController m_selfImpactCellController;    //自分が所属するセルを管理する。

        private Cell m_currentCell = null;

        public LostPatrol(EnemyBase owner) :
            this(owner, new Parametor() { time = 10.0f, seekParam = AstarSeek.DEFAULT_PARAMETOR })
        { }

        public LostPatrol(EnemyBase owner, Parametor parametor) :
            base(owner)
        {
            m_param = parametor;
            m_timer = new GameTimer();

            m_astarSeek = owner.GetComponent<AstarSeek>();
            m_selfAstarNodeController = owner.GetComponent<SelfAstarNodeController>();
            m_selfImpactCellController = owner.GetComponent<SelfImpactCellController>();

            Debug.Log("★LostPatrol");
        }

        protected override void ReserveChangeComponents()
        {
            base.ReserveChangeComponents();

            var owner = GetOwner();
            AddChangeComp(owner.GetComponent<AstarSeek>(), true, false);
        }

        public override void OnStart()
        {
            base.OnStart();

            //AstarSeekのパラメータを設定
            m_timer.ResetTimer(m_param.time);
            m_astarSeek.SetParametor(m_param.seekParam);

            StartAstar();   //Astarの開始
        }

        public override bool OnUpdate()
        {
            Debug.Log("★LostPatrol");

            m_timer.UpdateTimer();      //時間計測

            if (m_astarSeek.IsEnd()) {  //AstarSeekが終了したら
                StartAstar();
            }

            return IsEnd();              //一定時間たったら、探索を終了する。
        }

        private void StartAstar()
        {
            StartAstar_WayPoints();
        }

        private void StartAstar_CellMap()
        {
            //デバッグようにまだ残しておいて
            var factoryParametor = AIDirector.Instance.GetFieldWayPointsMap_FactoryParametor();
            var wayPointsMap = AIDirector.Instance.GetWayPointsMap().GetGraph();
            var selfNode = m_selfAstarNodeController.GetNode();
            var targetPosition = CalculateTargetPosition();

            //Astarの開始
            m_astarSeek.StartAstar(selfNode, targetPosition, wayPointsMap, factoryParametor.intervalRange);
        }

        private void StartAstar_WayPoints() {
            var wayPointsMap = AIDirector.Instance.GetWayPointsMap().GetGraph();
            var selfNode = m_selfAstarNodeController.GetNode();
            var targetNode = CalculateTargetNode();

            m_astarSeek.StartAstar(selfNode, targetNode, wayPointsMap);
        }

        private AstarNode CalculateTargetNode()
        {
            AstarNode result = null;

            var wayPointsMap = AIDirector.Instance.GetWayPointsMap();       //ウェイポイントマップ
            var selfNode = m_selfAstarNodeController.GetNode();             //現在のノード

            //ロストした場所付近で危険度が高い(まだ確認を行っていない)場所を取得する。
            var openDatas = new Queue<AstarNode>();
            openDatas.Enqueue(selfNode);
            var closeDatas = new Queue<AstarNode>();

            //openDataが空になるまで処理を続ける。
            while (openDatas.Count != 0)
            {
                var curretNode = openDatas.Dequeue();
                closeDatas.Enqueue(curretNode);

                var edges = wayPointsMap.GetGraph().GetEdges(curretNode.GetIndex());
                
                if(edges == null) {
                    continue;
                }

                foreach (var edge in edges)
                {
                    //追加OpenDataに追加できるかを判断
                    if (IsAddOpenData_AstarNode(openDatas, closeDatas, edge.GetToNode() as AstarNode))
                    {
                        openDatas.Enqueue(edge.GetToNode() as AstarNode);
                    }
                }
            }

            //クローズデータが存在しないなら、処理を停止
            if (closeDatas.Count == 0) {
                return result;
            }

            //オープンデータの中で一番危険度の高い場所を検索
            var sortCloseDatas = closeDatas.OrderByDescending(value => { return value.GetDangerValue(); });
            result = sortCloseDatas.ToArray()[0];

            return result;
        }

        private Vector3 CalculateTargetPosition()
        {
            var result = Vector3.zero;

            var cellMap = AIDirector.Instance.GetImpactCellMap();       //セルマップ
            var selfCell = m_selfImpactCellController.GetCurrentCell(); //現在のセル

            //ロストした場所付近で危険度が高い(まだ確認を行っていない)場所を取得する。
            var openDatas = new Queue<ImpactCell>();
            openDatas.Enqueue(selfCell);
            var closeDatas = new Queue<ImpactCell>();

            //openDataが空になるまで処理を続ける。
            while (openDatas.Count != 0)
            {
                var currentCell = openDatas.Dequeue();
                closeDatas.Enqueue(currentCell);

                //八方向のノードを取得する。
                var cells = cellMap.FindEightDirectionCells(currentCell.GetIndex());

                foreach (var cell in cells)
                {
                    //追加OpenDataに追加できるかを判断
                    if (IsAddOpenData(openDatas, closeDatas, cell)) {
                        openDatas.Enqueue(cell);
                    }
                }
            }

            //クローズデータが存在しないなら、処理を停止
            if (closeDatas.Count == 0) {
                return result;
            }

            //オープンデータの中で一番危険度の高い場所を検索
            var sortCloseDatas = closeDatas.OrderByDescending(value => { return value.GetDangerValue(); });
            result = sortCloseDatas.ToArray()[0].GetPosition();

            //デバッグ
            if (m_currentCell != null) {
                m_currentCell.IsTarget = false;
            }
            m_currentCell = sortCloseDatas.ToArray()[0];
            m_currentCell.IsTarget = true;

            return result;
        }

        private bool IsAddOpenData_AstarNode(Queue<AstarNode> openDatas, Queue<AstarNode> closeDatas, AstarNode node) {
            if (!node.IsActive()) {
                return false;
            }

            //一定距離以上なら処理をしない。
            const float Range = 15.0f;
            var range = (node.GetPosition() - GetOwner().transform.position).magnitude;
            if(range > Range) {
                return false;
            }

            if (openDatas.Contains(node)) {
                return false;
            }

            if (closeDatas.Contains(node)) {
                return false;
            }

            return true;
        }

        private bool IsAddOpenData(Queue<ImpactCell> openDatas, Queue<ImpactCell> closeDatas, ImpactCell currentCell)
        {
            if (!currentCell.IsActive()) {
                return false;
            }

            //一定距離以上なら追加しない
            const float Range = 15.0f;
            var range = (currentCell.GetPosition() - GetOwner().transform.position).magnitude;
            if (range > Range)
            {
                return false;
            }

            if (openDatas.Contains(currentCell))
            {
                return false;
            }

            if (closeDatas.Contains(currentCell))
            {
                return false;
            }

            //後ろ側なら処理をしない。

            return true;
        }

        private bool IsEnd() { return m_timer.IsTimeUp; }
    }

}
