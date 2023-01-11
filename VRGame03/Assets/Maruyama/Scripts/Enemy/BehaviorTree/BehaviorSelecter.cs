using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using System.Linq;

//--------------------------------------------------------------------------------------
/// ビヘイビアセレクター
//--------------------------------------------------------------------------------------
public class BehaviorSelecter : BehaviorNode
{
	//--------------------------------------------------------------------------------------
	/// ビヘイビアセレクターのセレクトタイプ
	//--------------------------------------------------------------------------------------
	public enum SelectType
	{
		Priority,   //優先度
		Sequence,   //シーケンス
		Random,     //ランダム
	};

	private BehaviorNode m_currentNode;             //現在使用中のノード

	private SelectType m_selectType;                //セレクトタイプ

	private BehaviorEdge m_fromEdge;                //手前のエッジ
	private List<BehaviorEdge> m_transitionEdges;   //遷移先のエッジ配列

	public BehaviorSelecter() :
		this(SelectType.Priority)
	{ }

	public BehaviorSelecter(SelectType selectType) {
		m_selectType = selectType;
	}

    public override void OnStart() { }

    public override bool OnUpdate() { return false; }

	public override void OnExit() {
		//遷移先のエッジを元の状態に戻す。
		foreach (var edge in m_transitionEdges)
		{
			edge.GetToNode().SetState(BehaviorState.Inactive);
		}

		SetCurrentNode(null);    //ノードをnullptrに変更		
	}

	/// <summary>
	/// 現在のノードを検索する
	/// </summary>
	/// <returns>現在のノード</returns>
	public BehaviorNode SearchCurrentNode()
	{
		switch (m_selectType)
		{
			case SelectType.Priority:
				return SearchFirstPriorityNode();
			case SelectType.Random:
				return SearchRandomNode();
			case SelectType.Sequence:
				return SearchSequenceNode();
		}

		return null;
	}

	/// <summary>
	/// 優先度の一番高いノードを取得する
	/// </summary>
	/// <returns>優先度の一番高いノード</returns>
	private BehaviorNode SearchFirstPriorityNode()
	{
		//現在のステートがRunningなら、一度検索をしているため、終了。
		if (IsState(BehaviorState.Running))
		{
			return null;
		}

		//遷移先ノードが空ならnullptr
		if (IsEmptyTransitionNodes())
		{
			return null;
		}

		var transitionEdges = m_transitionEdges;       //メンバをソートするとconstにできないため、一時変数化

		//エッジの優先度計算を先にする。
		foreach (var edge in transitionEdges)
		{
			if (edge == null)
			{   //存在しないなら
				continue;
			}

			edge.CalculatePriority();
		}

		//昇順ソート
		transitionEdges.Sort();
		//std::sort(transitionEdges.begin(), transitionEdges.end(), &SortEdges);

		//並べ替えたノードが遷移できるかどうかを判断する。
		foreach (var edge in transitionEdges)
		{
			if (edge.GetToNode().CanTransition())
			{   //遷移できるなら、そのノードを返す。
				return edge.GetToNode();
			}
		}

		return null;
	}

	private BehaviorNode SearchRandomNode() {
		List<BehaviorEdge> transitionEdges = new List<BehaviorEdge>();
		foreach (var edge in m_transitionEdges) {
			if (edge.GetToNode().CanTransition()) {
				transitionEdges.Add(edge);
			}
		}

		if (transitionEdges.Count == 0)
		{   //遷移先が存在しないならnullptrを返す。
			return null;
		}

		var randomEdge = MaruUtility.MyRandom.RandomList(transitionEdges);
		return randomEdge.GetToNode();
	}

	private BehaviorNode SearchSequenceNode() {
		//積まれた上から順に遷移できるタスクに遷移。
		foreach (var edge in m_transitionEdges) {
			var toNode = edge.GetToNode();
			if (toNode.CanTransition()) {
				return toNode;
			}
		}

		return null;
	}

	public void ChangeCurrentNode(BehaviorNode node) {
		//現在のノードの終了処理をする。
		var currentNode = GetCurrentNode();
		if (currentNode != null) {
			currentNode.OnExit();
		}

		m_currentNode = node;

		//変更するノードがnullでなかったら、開始処理をする。
		if (node != null)
		{
			node.OnStart();
		}
	}


	//--------------------------------------------------------------------------------------
	/// アクセッサ
	//--------------------------------------------------------------------------------------

	public void SetSelectType(SelectType type) { m_selectType = type; }

	public SelectType GetSelectType() { return m_selectType; }

	/// <summary>
	/// 手前エッジの設定
	/// </summary>
	public void SetFromEdge(BehaviorEdge fromEdge) { m_fromEdge = fromEdge; }

	/// <summary>
	/// 手前エッジの取得
	/// </summary>
	public BehaviorEdge GetFromEdge() { return m_fromEdge; }

	/// <summary>
	/// 遷移先エッジの追加
	/// </summary>
	public void AddTransitionEdge(BehaviorEdge edge) { m_transitionEdges.Add(edge); }

	/// <summary>
	/// 遷移先のノードが存在するかどうか
	/// </summary>
	/// <returns>存在するならtrue</returns>
	public bool IsEmptyTransitionNodes() { return m_transitionEdges.Count == 0; }

	/// <summary>
	/// カレントノードが存在するかどうか
	/// </summary>
	public bool HasCurrentNode() { return m_currentNode != null; }

	/// <summary>
	/// 現在積まれているノードを設定
	/// </summary>
	/// <param name="node">積まれているノード</param>
	public void SetCurrentNode(BehaviorNode node) { m_currentNode = node; }

	/// <summary>
	/// 現在使用中のノードを返す
	/// </summary>
	public BehaviorNode GetCurrentNode() { return m_currentNode; }

}
