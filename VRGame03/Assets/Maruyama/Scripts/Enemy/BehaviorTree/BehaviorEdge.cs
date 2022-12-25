using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BehaviorEdge
{
	private BehaviorNode m_fromNode;   //自分の手前のノード
	private BehaviorNode m_toNode;     //自分の先のノード

	private float m_priority;          //優先度

	public BehaviorEdge(
		BehaviorNode fromNode,
		BehaviorNode toNode,
		float priority
	) {
		m_fromNode = fromNode;
		m_toNode = toNode;
		m_priority = priority;
	}


	/// <summary>
	/// 手間のノードを設定
	/// </summary>
	/// <param name="node">手前のノード</param>
	public void SetFromNode(BehaviorNode node) { m_fromNode = node; }

	/// <summary>
	/// 手前のノードを取得
	/// </summary>
	/// <returns>手前のノード</returns>
	public BehaviorNode GetFromNode() { return m_fromNode; }

	/// <summary>
	/// 先のノードの設定
	/// </summary>
	/// <param name="node">先のノード</param>
	public void SetToNode(BehaviorNode node) { m_toNode = node; }

	/// <summary>
	/// 先のノードを取得
	/// </summary>
	/// <returns>先のノード</returns>
	public BehaviorNode GetToNode() { return m_toNode; }

	/// <summary>
	/// 優先度の設定
	/// </summary>
	/// <param name="priority">優先度</param>
	public void SetPriority(float priority) { m_priority = priority; }

	/// <summary>
	/// 優先度の取得
	/// </summary>
	/// <returns>優先度</returns>
	public float GetPriority() { return m_priority; }

	/// <summary>
	/// 優先度の計算
	/// </summary>
	/// <returns>計算後の優先度</returns>
	public float CalculatePriority() {
		//将来的に計算処理を入れる
		//現在はそのままの優先度を返す。

		return GetPriority();
	}
}
