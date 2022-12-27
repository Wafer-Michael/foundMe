using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using GraphType = SparseGraph<AstarNode, AstarEdge>;

namespace Factory
{
	public class WayPointsMap_FloodFill : MonoBehaviour
	{
		//方向マップ
		readonly Dictionary<DirectionType, Vector3> DIRECTION_MAP = new Dictionary<DirectionType, Vector3>() {
			{ DirectionType.Right,         Vector3.right },							//右
			{ DirectionType.RightForward,	(Vector3.right + Vector3.forward)},		//右奥
			{ DirectionType.RightBack,      (Vector3.right - Vector3.forward)},		//右手前

			{ DirectionType.Left,          -Vector3.right},							//左
			{ DirectionType.LeftForward,   (-Vector3.right + Vector3.forward) },	//左奥
			{ DirectionType.LeftBack,      (-Vector3.right - Vector3.forward) },	//左手前

			{ DirectionType.Foward,        Vector3.forward},						//左奥
			{ DirectionType.Back,          -Vector3.forward},						//左手前
		};

		/// <summary>
		/// 八方向の進行タイプ
		/// </summary>
		enum DirectionType
		{
			Right,
			RightForward,
			RightBack,
			Left,
			LeftForward,
			LeftBack,
			Foward,
			Back,
		}

		/// <summary>
		/// 方向タイプ別データ
		/// </summary>
		struct DataByDirectionType
		{
			public Vector3 direction; //方向
			public int plusIndex;  //加算するノードインデックス
		};

		/// <summary>
		/// 生成用のデータ
		/// </summary>
		struct OpenData
		{
			public AstarNode parentNode;    //自分の前のノード
			public AstarNode selfNode;    //自分自身のノード
			public bool isActive;                          //ノードが生きているかどうか

			public OpenData(
				AstarNode parentNode,
				AstarNode selfNode
			) {
				this.parentNode = parentNode;
				this.selfNode = selfNode;
				this.isActive = true;
			}
		}

		/// <summary>
		/// パラメータ
		/// </summary>
		[System.Serializable]
		public struct Parametor
		{
			public float intervalRange;     //ノードの間隔距離(5.0f)
			public maru.Rect rect;          //四角データ
			public float createHeight;      //高さ設定(0.5f)
		};

		Queue<OpenData> m_openDataQueue = new Queue<OpenData>();    //オープンデータキュー
		Dictionary<DirectionType, int> m_plusIndexMapByDirection = new Dictionary<DirectionType, int>();   //方向別の加算するインデックス数

		/// <summary>
		/// 方向データに合わせたインデックスの上限を設定する。
		/// </summary>
		/// <param name="parametor">パラメータ</param>
		/// <returns>方向データに合わせたインデックスの上限を設定する</returns>
		private Dictionary<DirectionType, int> SettingIndexByDirection(Parametor parametor)
		{
			Dictionary<DirectionType, int> result = new Dictionary<DirectionType, int>();

			maru.Rect rect = parametor.rect;
			float intervalRange = parametor.intervalRange;

			//基準となる横の大きさと、縦の大きさ
			int widthCount = (int)(rect.width / intervalRange);
			int plusIndex = widthCount + 1; //横の長さより一個分上で奥行き分のインデックスになる。

			result[DirectionType.Right] = +1;
			result[DirectionType.RightForward] = 1 + plusIndex;
			result[DirectionType.RightBack] = 1 - plusIndex;
			result[DirectionType.Left] = -1;
			result[DirectionType.LeftForward] = -1 + plusIndex;
			result[DirectionType.LeftBack] = -1 - plusIndex;
			result[DirectionType.Foward] = +plusIndex;
			result[DirectionType.Back] = -plusIndex;

			return result;
		}

		/// <summary>
		/// そのWayPointが生成できるかどうかを判断する。
		/// </summary>
		/// <param name="newOpenData">新規データ</param>
		/// <param name="graph">グラフ</param>
		/// <param name="parametor">生成パラメータ</param>
		/// <param name="isRayHit">障害物に当たったかどうかを参照したboolに保存する</param>
		/// <returns>生成できるならtrue</returns>
		private bool IsNodeCreate(
			OpenData newOpenData,
			GraphType graph,
			Parametor parametor,
			ref bool isRayHit
		)
		{
			var startPosition = newOpenData.parentNode.GetPosition();
			var targetPosition = newOpenData.selfNode.GetPosition();

			//ターゲットがエリアより外側にあるなら
			int testIndex = newOpenData.selfNode.GetIndex();
			var selfPosition = newOpenData.selfNode.GetPosition();
			if (!parametor.rect.IsInRect(newOpenData.selfNode.GetPosition())) {
				return false;
			}

			//障害物に当たっていたら(先に障害物判定をしないと、エッジと共有しているためバグる(修正検討中))
			//障害物のレイヤー判定
			if (isRayHit = maru.UtilityObstacle.IsRayObstacle(startPosition, targetPosition)) {
				return false;   //生成できない
			}

			//同じノードが存在するなら
			if (graph.IsSomeIndexNode(newOpenData.selfNode.GetIndex())) {
				return false;
			}

			return true;    //どの条件にも当てはまらないならtrue
		}

		/// <summary>
		/// エッジが生成できるかどうかを判断する。
		/// </summary>
		/// <param name="newOpenData">新規データ</param>
		/// <param name="graph">グラフ</param>
		/// <param name="parametor">生成パラメータ</param>
		/// <param name="isRayHit">障害物にヒットしたかどうか</param>
		/// <returns>生成できるならtrue</returns>
		private bool IsEdgeCreate(
			OpenData newOpenData,
			GraphType graph,
			Parametor parametor,
			bool isRayHit
		) {
			//障害物に当たっているなら、生成しない
			if (isRayHit)
			{
				return false;
			}

			//ターゲットがエリアより外側にあるなら
			int testIndex = newOpenData.selfNode.GetIndex();
			var selfPosition = newOpenData.selfNode.GetPosition();
			if (!parametor.rect.IsInRect(newOpenData.selfNode.GetPosition()))
			{
				return false;
			}

			//同じエッジが存在するなら
			var parentNode = newOpenData.parentNode;
			var selfNode = newOpenData.selfNode;
			if (graph.IsSomeIndexEdge(parentNode.GetIndex(), selfNode.GetIndex()))
			{
				return false;   //生成しない
			}

			return true;    //条件が通ったため、生成する。
		}

		/// <summary>
		/// ウェイポイントの複数生成
		/// </summary>
		/// <param name="startPosition">開始位置</param>
		/// <param name="parametor">生成パラメータ</param>
		private void CreateWayPoints(
			OpenData parentOpenData,
			GraphType graph,
			Parametor parametor
		){
			var openDatas = CreateChildrenOpenDatas(parentOpenData, parametor);    //オープンデータの生成

			//ループして、オープンデータの中から生成できるものを設定
			foreach (var openData in openDatas) {
				var parentNode = openData.parentNode;
				var selfNode = openData.selfNode;
				bool isRayHit = false;  //障害物に当たったかどうかを記録する。

				//ノードが生成できるなら
				if (IsNodeCreate(openData, graph, parametor,ref isRayHit))
				{
					var node = graph.AddNode(openData.selfNode); //グラフにノード追加
					m_openDataQueue.Enqueue(openData);                 //生成キューにOpenDataを追加
				}

				//エッジの生成条件がそろっているなら
				if (IsEdgeCreate(openData, graph, parametor, isRayHit))
				{
					var fromNode = graph.GetNode(parentNode.GetIndex());
					var toNode = graph.GetNode(selfNode.GetIndex());
					graph.AddEdge(new AstarEdge(fromNode, toNode));   //グラフにエッジ追加
				}
			}
		}

		/// <summary>
		/// インデックスを計算して返す
		/// </summary>
		/// <param name="parentOpenData">親となるオープンデータ</param>
		/// <param name="directionType">生成する方向データ</param>
		/// <returns></returns>
		int CalculateIndex(OpenData parentOpenData, DirectionType directionType)
        {
			var parent = parentOpenData.selfNode;
			int index = parent.GetIndex() + m_plusIndexMapByDirection[directionType];   //インデックスの計算
			return index;
		}

		/// <summary>
		/// 八方向のOpenDataを生成する。
		/// </summary>
		/// <param name="parentOpenData">親となるオープンデータ</param>
		/// <param name="parametor">パラメータ</param>
		/// <returns>八方向のOpenDataを生成する</returns>
		private List<OpenData> CreateChildrenOpenDatas(
			OpenData parentOpenData,
			Parametor parametor
        ) {
			List<OpenData> result = new List<OpenData>();
			var parent = parentOpenData.selfNode;	//親ノードを取得

			foreach (var pair in DIRECTION_MAP) {
				DirectionType directionType = pair.Key;	//方向タイプ
				Vector3 direction = pair.Value;				//方向ベクトル

				int index = CalculateIndex(parentOpenData, directionType);			//インデックスの計算
				if (index < 0) {	//インデックスが0より小さいなら処理を飛ばす。
					continue;
				}

				Vector3 startPosition = parent.GetPosition();			//開始位置
				Vector3 targetPosition = startPosition + (direction * parametor.intervalRange);	//生成位置

				var newNode = new AstarNode(index, targetPosition);	//新規ノードの作成

				var newOpenData = new OpenData(parent, newNode);		//新規データ作成
				result.Add(newOpenData);
			}

			return result;
        }

		/// <summary>
		/// ウェイポイントの生成
		/// </summary>
		/// <param name="startPosition">開始位置</param>
		/// <param name="graph">生成したいグラフ</param>
		/// <param name="parametor">生成パラメータ</param>
		public void AddWayPointMap(
			GraphType graph,
			Parametor parametor
		){
			m_plusIndexMapByDirection = SettingIndexByDirection(parametor); //方向別の加算するインデックス数をセッティング

			var baseStartPosition = parametor.rect.CalculateStartPosition();

			m_openDataQueue.Clear();
			var newNode = new AstarNode(0, baseStartPosition);
			//Debug::GetInstance()->Log(newNode->GetPosition());
			graph.AddNode(newNode);
			m_openDataQueue.Enqueue(new OpenData(null, newNode));

			while (m_openDataQueue.Count != 0)
			{   //キューが空になるまで
				var parentData = m_openDataQueue.Dequeue();
				//m_openDataQueue.pop();
				CreateWayPoints(parentData, graph, parametor);
			}
		}

	}
}

