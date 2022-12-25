using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum BehaviorState
{
	Inactive,   //非アクティブ(待機状態)
	Success,    //成功
	Failure,    //失敗
	Running,    //実行中
	Completed,  //完了
}

public class BehaviorTree<EnumType>
where EnumType : System.Enum
{
	private EnumType m_firstNodeType;                               //初回ノードタイプ
	private BehaviorNode m_currentNode;                             //現在積まれているタスク

	private Dictionary<EnumType, BehaviorNode> m_nodeMap;           //定義したノード
	private Dictionary<EnumType, BehaviorSelecter> m_selecterMap;   //定義したセレクター
	private Dictionary<EnumType, BehaviorTask> m_taskMap;           //定義したタスク

	private Stack<BehaviorNode> m_currentStack;   //現在積まれているノードスタック

	private Dictionary<EnumType, List<BehaviorEdge>> m_edgesMap;    //遷移エッジ


	/// <summary>
	/// ノードの追加
	/// </summary>
	/// <param name="type">ノードタイプ</param>
	/// <param name="node">ノード</param>
	private BehaviorNode AddNode(EnumType type, BehaviorNode node)
	{
		var typeInt = (int)(object)type;
		node.SetIndex(typeInt);
		m_nodeMap[type] = node;
		return node;
	}

	/// <summary>
	/// セレクターとエッジの結合
	/// </summary>
	/// <param name="edge"></param>
	private void Union(BehaviorEdge edge)
	{
		BehaviorNode fromNode = edge.GetFromNode();
		BehaviorNode toNode = edge.GetToNode();

		//エッジの手前ノードがセレクターであるなら、遷移先ノード情報を設定
		var fromSelecter = GetSelecter(fromNode.GetType<EnumType>());
		if (fromSelecter != null)
		{
			fromSelecter.AddTransitionEdge(edge);
		}

		//遷移先のノードがセレクターなら、手前ノードを登録する。
		var toSelecter = GetSelecter(toNode.GetType<EnumType>());
		if (toSelecter != null)
		{
			toSelecter.SetFromEdge(edge);
		}
	}

	/// <summary>
	/// カレントスタックに追加する。
	/// </summary>
	/// <param name="node"></param>
	private void AddCurrentStack(BehaviorNode node)
	{
		//ノードが存在するなら追加処理をする。
		if (node != null)
		{
			node.OnDecoratorStart();
			node.OnStart();
			node.SetState(BehaviorState.Running);
			m_currentStack.Push(node);
		}
	}

	/// <summary>
	/// カレントスタックからポップする。
	/// </summary>
	private void PopCurrentStack()
	{
		if (m_currentStack.Count == 0)
		{   //スタックが空なら処理をしない。
			return;
		}

		var node = m_currentStack.Peek();
		if (node != null)
		{
			node.OnDecoratorExit();
			node.OnExit();     //ノードの終了判定処理
			node.SetState(BehaviorState.Completed);
		}

		m_currentStack.Pop();
	}

	/// <summary>
	///	初期ノードのリセット
	/// </summary>
	private void ResetFirstNode()
	{
		foreach (var edge in GetEdges(m_firstNodeType))
		{
			edge.GetToNode().SetState(BehaviorState.Inactive);
		}
	}

	/// <summary>
	/// 全てのスタックされたノードの終了処理をする。
	/// </summary>
	private void ResetAllStack()
	{
		while (m_currentStack.Count != 0)
		{   //スタックが空になるまでループ
			PopCurrentStack();
		}
	}

	/// <summary>
	/// 現在のタスクのタイプ
	/// </summary>
	/// <returns>現在のタスクタイプ</returns>
	public EnumType GetCurrentType() { return m_currentNode != null ? m_currentNode.GetType<EnumType>() : (EnumType)(object)(0); }

	/// <summary>
	/// 現在のノードを設定
	/// </summary>
	/// <param name="node">現在のノード</param>
	public void SetCurrentNode(BehaviorNode node) { m_currentNode = node; }

	/// <summary>
	/// 現在のノードを取得
	/// </summary>
	/// <returns>現在のノード</returns>
	public BehaviorNode GetCurrentNode() { return m_currentNode; }

	/// <summary>
	/// そのノードが存在するかどうか
	/// </summary>
	/// <param name="type">確認したいタイプ</param>
	/// <returns>ノードが存在するならtrue</returns>
	public bool HasNode(EnumType type) { return m_nodeMap.ContainsKey(type); }

	/// <summary>
	/// ノードの取得
	/// </summary>
	/// <param name="type">ノードのタイプ</param>
	/// <returns>取得したノード</returns>
	public BehaviorNode GetNode(EnumType type) { return m_nodeMap.ContainsKey(type) ? m_nodeMap[type] : null; }

	/// <summary>
	/// 渡したノードの遷移先ノードを取得
	/// </summary>
	/// <param name="node">遷移先を取得したいノード</param>
	/// <returns>最優先の遷移先ノード</returns>
	public BehaviorNode GetTransitionNode(BehaviorNode node)
	{
		//Selecterであることを保証する。
		var selecter = GetSelecter(node.GetType<EnumType>());
		if (selecter == null)
		{
			return null;
		}

		//セレクターのカレントノードを検索して取得
		return selecter.SearchCurrentNode();
	}

	/// <summary>
	/// 指定したタイプのSelecterを持っているかどうか
	/// </summary>
	/// <param name="type">指定タイプ</param>
	/// <returns>持っているならtrue</returns>
	public bool HasSelecter(EnumType type) { return m_selecterMap.ContainsKey(type); }

	/// <summary>
	/// セレクターの追加
	/// </summary>
	/// <param name="type">ノードタイプ</param>
	public void AddSelecter(EnumType type) { AddSelecter(type, new BehaviorSelecter()); }

	/// <summary>
	/// セレクターの追加
	/// </summary>
	/// <param name="type">ノードタイプ</param>
	/// <param name="selecter">セレクター</param>
	public void AddSelecter(EnumType type, BehaviorSelecter selecter)
	{
		m_selecterMap[type] = selecter;
		AddNode(type, selecter);
	}

	/// <summary>
	/// セレクターの取得
	/// </summary>
	/// <returns>セレクター</returns>
	BehaviorSelecter GetSelecter(EnumType type) { return HasSelecter(type) ? m_selecterMap[type] : null; }  //持っていないならnullptrを返す。

	/// <summary>
	/// タスクが定義されているかどうか
	/// </summary>
	/// <param name="type">タスクタイプ</param>
	/// <returns>タスクが定義されていたらtrue</returns>
	bool HasTask(EnumType type) { return m_taskMap.ContainsKey(type); }

	/// <summary>
	/// タスクの追加
	/// </summary>
	/// <param name="type">ノードタイプ</param>
	/// <param name="node">タスク</param>
	public void AddTask(EnumType type, BehaviorTask task)
	{
		m_taskMap[type] = task;
		task.SetIndex((int)(object)type);
		AddNode(type, task);
	}

	/// <summary>
	/// タスクの取得
	/// </summary>
	/// <param name="type">ノードタイプ</param>
	public BehaviorTask GetTask(EnumType type) { return HasTask(type) ? m_taskMap[type] : null; }

	/// <summary>
	/// 現在のノードを取得
	/// </summary>
	public BehaviorTask GetCurrentTask() { return m_currentNode as BehaviorTask; }

	/// <summary>
	/// エッジの追加
	/// </summary>
	/// <param name="fromType">手前のノードタイプ</param>
	/// <param name="toType">遷移先のノードタイプ</param>
	/// <param name="priority">優先度</param>
	public BehaviorEdge AddEdge(EnumType fromType, EnumType toType, float priority)
	{
		return new BehaviorEdge(GetNode(fromType), GetNode(toType), priority);
	}

	/// <summary>
	/// エッジの取得
	/// </summary>
	/// <param name="fromType">エッジのFromタイプ</param>
	/// <param name="toType">エッジのToタイプ</param>
	/// <returns>エッジ</returns>
	public BehaviorEdge GetEdge(EnumType fromType, EnumType toType)
	{
		var edges = GetEdges(fromType);
		foreach (var edge in edges)
		{
			int toIndex = (int)(object)(toType);
			int toEdgeIndex = edge.GetToNode().GetIndex();
			if (toIndex == toEdgeIndex) {
				return edge;
			}
		}

		return null;
	}

	/// <summary>
	/// 引数のタイプから伸びるエッジ配列の取得
	/// </summary>
	/// <param name="type">タイプ</param>
	/// <returns>引数のタイプから伸びるエッジ配列</returns>
	public List<BehaviorEdge> GetEdges(EnumType type)
	{
		if (!HasEdges(type))
		{   //そのタイプのエッジが存在しないなら空の配列を返す。
			return new List<BehaviorEdge>();
		}

		return m_edgesMap[type];
	}

	/// <summary>
	/// エッジを持っているかどうか
	/// </summary>
	/// <param name="fromType">エッジのFromタイプ</param>
	/// <param name="toType">エッジのToタイプ</param>
	/// <returns>エッジがあるならtrue</returns>
	public bool HasEdge(EnumType fromType, EnumType toType)
	{
		var edges = GetEdges(fromType);
		foreach (var edge in edges)
		{
			int toIndex = (int)(object)(toType);
			int toEdgeIndex = edge.GetToNode().GetIndex();
			if (toIndex == toEdgeIndex)
			{
				return true;
			}
		}

		return false;
	}

	/// <summary>
	/// エッジを持っているかどうか
	/// </summary>
	/// <param name="type">エッジのFromタイプ</param>
	/// <returns>エッジがあるならtrue</returns>
	public bool HasEdges(EnumType type) { return m_edgesMap.ContainsKey(type); }

	/// <summary>
	/// デコレータの設定
	/// </summary>
	/// <param name="type">設定したいノ―ドタイプ</param>
	/// <param name="decorator">設定したいデコレータ</param>
	public bool AddDecorator(EnumType type, I_Decorator decorator)
	{
		if (!HasNode(type))
		{
			return false;
		}

		var node = GetNode(type);
		decorator.OnCreate();          //生成時に一度だけ呼び出したい処理。
		node.AddDecorator(decorator);

		return true;
	}

	/// <summary>
	/// 空の状態かどうか
	/// </summary>
	/// <returns>空の状態ならtrue</returns>
	public bool IsEmpty() { return m_taskMap.Count == 0; }

	/// <summary>
	/// 開始ノードタイプの設定
	/// </summary>
	/// <param name="type">開始ノードタイプ</param>
	public void SetFirstNodeType(EnumType type) { m_firstNodeType = type; }

	/// <summary>
	/// 開始ノードの取得
	/// </summary>
	/// <returns>開始ノード</returns>
	public EnumType GetFirstNodeType() { return m_firstNodeType; }

	/// <summary>
	/// 強制終了
	/// </summary>
	public void ForceStop()
	{
		ResetAllStack();
		ResetFirstNode();
		m_currentNode = null;
	}

	/// <summary>
	/// ノードの更新
	/// </summary>
	/// <returns>ノードの更新が終了したならtrue</returns>
	private bool TaskUpdate()
	{
		var currentTask = GetCurrentTask();
		if (currentTask != null)
		{
			return currentTask.OnUpdate();
		}

		return true;
	}

	/// <summary>
	/// 更新不可になっているノードが存在するかどうか
	/// </summary>
	/// <returns>更新不可のノードを返す</returns>
	private BehaviorNode SearchStopNode()
	{
		var copyStack = m_currentStack;
		while (copyStack.Count != 0)
		{   //スタックが空になるまで
			var node = copyStack.Peek();
			if (!node.CanUpdate())
			{   //ノードが更新可能でなかったら
				return node;
			}

			copyStack.Pop();
		}

		return null;
	}

	/// <summary>
	/// 再起して巻き戻している時の、Pop処理と次のノードを返す処理
	/// </summary>
	/// <returns>次のノード</returns>
	private BehaviorNode ReverseProcess()
	{
		PopCurrentStack();

		if (m_currentStack.Count == 0)
		{   //スタックが0になったら初期ノードを返す。
			return GetNode(m_firstNodeType);
		}

		return m_currentStack.Peek();
	}

	/// <summary>
	/// 遷移先のノードが見つかるまで、スタックを巻き戻す。
	/// </summary>
	/// <param name="node">確認したいノード</param>
	/// <returns></returns>
	private BehaviorNode ReverseStack(BehaviorNode node)
	{
		if (node == GetNode(m_firstNodeType))
		{
			ResetFirstNode();
		}

		var selecter = GetSelecter(node.GetType<EnumType>());
		if (selecter == null)
		{   //セレクターでないなら前のノードに戻る。
			return ReverseProcess();
		}

		var nextNode = selecter.SearchCurrentNode();
		if (nextNode == null)
		{   //ノードが存在しないなら、手前のノードに戻る。
			return ReverseProcess();
		}

		return nextNode;
	}

	/// <summary>
	/// 再起処理をして、遷移先のノードを取得する。
	/// </summary>
	/// <returns>一番優先度の高いノード</returns>
	private BehaviorNode Recursive_TransitionNode(BehaviorNode node)
	{
		if (node == null) {
			return null;
		}

		//Taskノードなら、末端ノードとなる。
		if (HasTask(node.GetType<EnumType>())) {
			AddCurrentStack(node);  //最終ノードをスタックに入れる。
			return node;            //末端ノードが確定
		}

		//遷移先が存在するなら、スタックに追加して再起処理
		var transitionNode = GetTransitionNode(node);
		if (transitionNode != null) {
			AddCurrentStack(node);  //スタックに積む。
									//一番優先順位の高いノードを取得する。
			return Recursive_TransitionNode(transitionNode);
		}

		//遷移先がないため、Popして戻る。
		return Recursive_TransitionNode(ReverseStack(node));
	}

	/// <summary>
	/// 引数で渡したノードの手前ノードまでスタックを戻す
	/// </summary>
	/// <param name="targetNode">戻りたいノード</param>
	private void ReverseTargetBeforeStack(BehaviorNode targetNode)
	{
		if (m_currentStack.Count == 0) {
			return;
		}

		var currentNode = m_currentStack.Peek();
		if (currentNode == targetNode) {
			PopCurrentStack();  //ストップした手前のノードまで戻って終了
			return;
		}

		PopCurrentStack();      //スタックをポップ
		ReverseTargetBeforeStack(targetNode);   //再起処理
	}

	/// <summary>
	/// 遷移するときの判断開始位置のノードを取得する。
	/// </summary>
	private BehaviorNode GetTransitionStartNode()
	{
		if (m_currentStack.Count == 0)
		{
			return GetNode(m_firstNodeType);
		}

		return ReverseStack(m_currentStack.Peek());
	}

	/// <summary>
	/// 遷移処理
	/// </summary>
	private void Transition()
	{
		//優先度の一番高いノードに遷移
		var node = Recursive_TransitionNode(GetTransitionStartNode());
		SetCurrentNode(node);   //カレントノードの設定
	}

	/// <summary>
	/// 更新処理
	/// </summary>
	public void OnUpdate()
	{
		if (IsEmpty()) {
			return;
		}

		//監視が必要なデコレータの更新
		var stopNode = SearchStopNode();
		if (stopNode != null)
		{   //戻り値が存在するなら、更新不能なノードが存在した。
			//ストップしたノードの手前ノードまで戻る。
			ReverseTargetBeforeStack(stopNode);
			Transition();   //遷移
		}

		bool isEndTaskUpdate = TaskUpdate();    //ノードの更新

		//ノードが終了したら、遷移
		if (isEndTaskUpdate) {
			Transition();   //遷移
		}
	}
}







