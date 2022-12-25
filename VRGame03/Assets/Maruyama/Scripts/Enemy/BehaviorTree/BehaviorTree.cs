using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum BehaviorState
{
	Inactive,   //��A�N�e�B�u(�ҋ@���)
	Success,    //����
	Failure,    //���s
	Running,    //���s��
	Completed,  //����
}

public class BehaviorTree<EnumType>
where EnumType : System.Enum
{
	private EnumType m_firstNodeType;                               //����m�[�h�^�C�v
	private BehaviorNode m_currentNode;                             //���ݐς܂�Ă���^�X�N

	private Dictionary<EnumType, BehaviorNode> m_nodeMap;           //��`�����m�[�h
	private Dictionary<EnumType, BehaviorSelecter> m_selecterMap;   //��`�����Z���N�^�[
	private Dictionary<EnumType, BehaviorTask> m_taskMap;           //��`�����^�X�N

	private Stack<BehaviorNode> m_currentStack;   //���ݐς܂�Ă���m�[�h�X�^�b�N

	private Dictionary<EnumType, List<BehaviorEdge>> m_edgesMap;    //�J�ڃG�b�W


	/// <summary>
	/// �m�[�h�̒ǉ�
	/// </summary>
	/// <param name="type">�m�[�h�^�C�v</param>
	/// <param name="node">�m�[�h</param>
	private BehaviorNode AddNode(EnumType type, BehaviorNode node)
	{
		var typeInt = (int)(object)type;
		node.SetIndex(typeInt);
		m_nodeMap[type] = node;
		return node;
	}

	/// <summary>
	/// �Z���N�^�[�ƃG�b�W�̌���
	/// </summary>
	/// <param name="edge"></param>
	private void Union(BehaviorEdge edge)
	{
		BehaviorNode fromNode = edge.GetFromNode();
		BehaviorNode toNode = edge.GetToNode();

		//�G�b�W�̎�O�m�[�h���Z���N�^�[�ł���Ȃ�A�J�ڐ�m�[�h����ݒ�
		var fromSelecter = GetSelecter(fromNode.GetType<EnumType>());
		if (fromSelecter != null)
		{
			fromSelecter.AddTransitionEdge(edge);
		}

		//�J�ڐ�̃m�[�h���Z���N�^�[�Ȃ�A��O�m�[�h��o�^����B
		var toSelecter = GetSelecter(toNode.GetType<EnumType>());
		if (toSelecter != null)
		{
			toSelecter.SetFromEdge(edge);
		}
	}

	/// <summary>
	/// �J�����g�X�^�b�N�ɒǉ�����B
	/// </summary>
	/// <param name="node"></param>
	private void AddCurrentStack(BehaviorNode node)
	{
		//�m�[�h�����݂���Ȃ�ǉ�����������B
		if (node != null)
		{
			node.OnDecoratorStart();
			node.OnStart();
			node.SetState(BehaviorState.Running);
			m_currentStack.Push(node);
		}
	}

	/// <summary>
	/// �J�����g�X�^�b�N����|�b�v����B
	/// </summary>
	private void PopCurrentStack()
	{
		if (m_currentStack.Count == 0)
		{   //�X�^�b�N����Ȃ珈�������Ȃ��B
			return;
		}

		var node = m_currentStack.Peek();
		if (node != null)
		{
			node.OnDecoratorExit();
			node.OnExit();     //�m�[�h�̏I�����菈��
			node.SetState(BehaviorState.Completed);
		}

		m_currentStack.Pop();
	}

	/// <summary>
	///	�����m�[�h�̃��Z�b�g
	/// </summary>
	private void ResetFirstNode()
	{
		foreach (var edge in GetEdges(m_firstNodeType))
		{
			edge.GetToNode().SetState(BehaviorState.Inactive);
		}
	}

	/// <summary>
	/// �S�ẴX�^�b�N���ꂽ�m�[�h�̏I������������B
	/// </summary>
	private void ResetAllStack()
	{
		while (m_currentStack.Count != 0)
		{   //�X�^�b�N����ɂȂ�܂Ń��[�v
			PopCurrentStack();
		}
	}

	/// <summary>
	/// ���݂̃^�X�N�̃^�C�v
	/// </summary>
	/// <returns>���݂̃^�X�N�^�C�v</returns>
	public EnumType GetCurrentType() { return m_currentNode != null ? m_currentNode.GetType<EnumType>() : (EnumType)(object)(0); }

	/// <summary>
	/// ���݂̃m�[�h��ݒ�
	/// </summary>
	/// <param name="node">���݂̃m�[�h</param>
	public void SetCurrentNode(BehaviorNode node) { m_currentNode = node; }

	/// <summary>
	/// ���݂̃m�[�h���擾
	/// </summary>
	/// <returns>���݂̃m�[�h</returns>
	public BehaviorNode GetCurrentNode() { return m_currentNode; }

	/// <summary>
	/// ���̃m�[�h�����݂��邩�ǂ���
	/// </summary>
	/// <param name="type">�m�F�������^�C�v</param>
	/// <returns>�m�[�h�����݂���Ȃ�true</returns>
	public bool HasNode(EnumType type) { return m_nodeMap.ContainsKey(type); }

	/// <summary>
	/// �m�[�h�̎擾
	/// </summary>
	/// <param name="type">�m�[�h�̃^�C�v</param>
	/// <returns>�擾�����m�[�h</returns>
	public BehaviorNode GetNode(EnumType type) { return m_nodeMap.ContainsKey(type) ? m_nodeMap[type] : null; }

	/// <summary>
	/// �n�����m�[�h�̑J�ڐ�m�[�h���擾
	/// </summary>
	/// <param name="node">�J�ڐ���擾�������m�[�h</param>
	/// <returns>�ŗD��̑J�ڐ�m�[�h</returns>
	public BehaviorNode GetTransitionNode(BehaviorNode node)
	{
		//Selecter�ł��邱�Ƃ�ۏ؂���B
		var selecter = GetSelecter(node.GetType<EnumType>());
		if (selecter == null)
		{
			return null;
		}

		//�Z���N�^�[�̃J�����g�m�[�h���������Ď擾
		return selecter.SearchCurrentNode();
	}

	/// <summary>
	/// �w�肵���^�C�v��Selecter�������Ă��邩�ǂ���
	/// </summary>
	/// <param name="type">�w��^�C�v</param>
	/// <returns>�����Ă���Ȃ�true</returns>
	public bool HasSelecter(EnumType type) { return m_selecterMap.ContainsKey(type); }

	/// <summary>
	/// �Z���N�^�[�̒ǉ�
	/// </summary>
	/// <param name="type">�m�[�h�^�C�v</param>
	public void AddSelecter(EnumType type) { AddSelecter(type, new BehaviorSelecter()); }

	/// <summary>
	/// �Z���N�^�[�̒ǉ�
	/// </summary>
	/// <param name="type">�m�[�h�^�C�v</param>
	/// <param name="selecter">�Z���N�^�[</param>
	public void AddSelecter(EnumType type, BehaviorSelecter selecter)
	{
		m_selecterMap[type] = selecter;
		AddNode(type, selecter);
	}

	/// <summary>
	/// �Z���N�^�[�̎擾
	/// </summary>
	/// <returns>�Z���N�^�[</returns>
	BehaviorSelecter GetSelecter(EnumType type) { return HasSelecter(type) ? m_selecterMap[type] : null; }  //�����Ă��Ȃ��Ȃ�nullptr��Ԃ��B

	/// <summary>
	/// �^�X�N����`����Ă��邩�ǂ���
	/// </summary>
	/// <param name="type">�^�X�N�^�C�v</param>
	/// <returns>�^�X�N����`����Ă�����true</returns>
	bool HasTask(EnumType type) { return m_taskMap.ContainsKey(type); }

	/// <summary>
	/// �^�X�N�̒ǉ�
	/// </summary>
	/// <param name="type">�m�[�h�^�C�v</param>
	/// <param name="node">�^�X�N</param>
	public void AddTask(EnumType type, BehaviorTask task)
	{
		m_taskMap[type] = task;
		task.SetIndex((int)(object)type);
		AddNode(type, task);
	}

	/// <summary>
	/// �^�X�N�̎擾
	/// </summary>
	/// <param name="type">�m�[�h�^�C�v</param>
	public BehaviorTask GetTask(EnumType type) { return HasTask(type) ? m_taskMap[type] : null; }

	/// <summary>
	/// ���݂̃m�[�h���擾
	/// </summary>
	public BehaviorTask GetCurrentTask() { return m_currentNode as BehaviorTask; }

	/// <summary>
	/// �G�b�W�̒ǉ�
	/// </summary>
	/// <param name="fromType">��O�̃m�[�h�^�C�v</param>
	/// <param name="toType">�J�ڐ�̃m�[�h�^�C�v</param>
	/// <param name="priority">�D��x</param>
	public BehaviorEdge AddEdge(EnumType fromType, EnumType toType, float priority)
	{
		return new BehaviorEdge(GetNode(fromType), GetNode(toType), priority);
	}

	/// <summary>
	/// �G�b�W�̎擾
	/// </summary>
	/// <param name="fromType">�G�b�W��From�^�C�v</param>
	/// <param name="toType">�G�b�W��To�^�C�v</param>
	/// <returns>�G�b�W</returns>
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
	/// �����̃^�C�v����L�т�G�b�W�z��̎擾
	/// </summary>
	/// <param name="type">�^�C�v</param>
	/// <returns>�����̃^�C�v����L�т�G�b�W�z��</returns>
	public List<BehaviorEdge> GetEdges(EnumType type)
	{
		if (!HasEdges(type))
		{   //���̃^�C�v�̃G�b�W�����݂��Ȃ��Ȃ��̔z���Ԃ��B
			return new List<BehaviorEdge>();
		}

		return m_edgesMap[type];
	}

	/// <summary>
	/// �G�b�W�������Ă��邩�ǂ���
	/// </summary>
	/// <param name="fromType">�G�b�W��From�^�C�v</param>
	/// <param name="toType">�G�b�W��To�^�C�v</param>
	/// <returns>�G�b�W������Ȃ�true</returns>
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
	/// �G�b�W�������Ă��邩�ǂ���
	/// </summary>
	/// <param name="type">�G�b�W��From�^�C�v</param>
	/// <returns>�G�b�W������Ȃ�true</returns>
	public bool HasEdges(EnumType type) { return m_edgesMap.ContainsKey(type); }

	/// <summary>
	/// �f�R���[�^�̐ݒ�
	/// </summary>
	/// <param name="type">�ݒ肵�����m�\�h�^�C�v</param>
	/// <param name="decorator">�ݒ肵�����f�R���[�^</param>
	public bool AddDecorator(EnumType type, I_Decorator decorator)
	{
		if (!HasNode(type))
		{
			return false;
		}

		var node = GetNode(type);
		decorator.OnCreate();          //�������Ɉ�x�����Ăяo�����������B
		node.AddDecorator(decorator);

		return true;
	}

	/// <summary>
	/// ��̏�Ԃ��ǂ���
	/// </summary>
	/// <returns>��̏�ԂȂ�true</returns>
	public bool IsEmpty() { return m_taskMap.Count == 0; }

	/// <summary>
	/// �J�n�m�[�h�^�C�v�̐ݒ�
	/// </summary>
	/// <param name="type">�J�n�m�[�h�^�C�v</param>
	public void SetFirstNodeType(EnumType type) { m_firstNodeType = type; }

	/// <summary>
	/// �J�n�m�[�h�̎擾
	/// </summary>
	/// <returns>�J�n�m�[�h</returns>
	public EnumType GetFirstNodeType() { return m_firstNodeType; }

	/// <summary>
	/// �����I��
	/// </summary>
	public void ForceStop()
	{
		ResetAllStack();
		ResetFirstNode();
		m_currentNode = null;
	}

	/// <summary>
	/// �m�[�h�̍X�V
	/// </summary>
	/// <returns>�m�[�h�̍X�V���I�������Ȃ�true</returns>
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
	/// �X�V�s�ɂȂ��Ă���m�[�h�����݂��邩�ǂ���
	/// </summary>
	/// <returns>�X�V�s�̃m�[�h��Ԃ�</returns>
	private BehaviorNode SearchStopNode()
	{
		var copyStack = m_currentStack;
		while (copyStack.Count != 0)
		{   //�X�^�b�N����ɂȂ�܂�
			var node = copyStack.Peek();
			if (!node.CanUpdate())
			{   //�m�[�h���X�V�\�łȂ�������
				return node;
			}

			copyStack.Pop();
		}

		return null;
	}

	/// <summary>
	/// �ċN���Ċ����߂��Ă��鎞�́APop�����Ǝ��̃m�[�h��Ԃ�����
	/// </summary>
	/// <returns>���̃m�[�h</returns>
	private BehaviorNode ReverseProcess()
	{
		PopCurrentStack();

		if (m_currentStack.Count == 0)
		{   //�X�^�b�N��0�ɂȂ����珉���m�[�h��Ԃ��B
			return GetNode(m_firstNodeType);
		}

		return m_currentStack.Peek();
	}

	/// <summary>
	/// �J�ڐ�̃m�[�h��������܂ŁA�X�^�b�N�������߂��B
	/// </summary>
	/// <param name="node">�m�F�������m�[�h</param>
	/// <returns></returns>
	private BehaviorNode ReverseStack(BehaviorNode node)
	{
		if (node == GetNode(m_firstNodeType))
		{
			ResetFirstNode();
		}

		var selecter = GetSelecter(node.GetType<EnumType>());
		if (selecter == null)
		{   //�Z���N�^�[�łȂ��Ȃ�O�̃m�[�h�ɖ߂�B
			return ReverseProcess();
		}

		var nextNode = selecter.SearchCurrentNode();
		if (nextNode == null)
		{   //�m�[�h�����݂��Ȃ��Ȃ�A��O�̃m�[�h�ɖ߂�B
			return ReverseProcess();
		}

		return nextNode;
	}

	/// <summary>
	/// �ċN���������āA�J�ڐ�̃m�[�h���擾����B
	/// </summary>
	/// <returns>��ԗD��x�̍����m�[�h</returns>
	private BehaviorNode Recursive_TransitionNode(BehaviorNode node)
	{
		if (node == null) {
			return null;
		}

		//Task�m�[�h�Ȃ�A���[�m�[�h�ƂȂ�B
		if (HasTask(node.GetType<EnumType>())) {
			AddCurrentStack(node);  //�ŏI�m�[�h���X�^�b�N�ɓ����B
			return node;            //���[�m�[�h���m��
		}

		//�J�ڐ悪���݂���Ȃ�A�X�^�b�N�ɒǉ����čċN����
		var transitionNode = GetTransitionNode(node);
		if (transitionNode != null) {
			AddCurrentStack(node);  //�X�^�b�N�ɐςށB
									//��ԗD�揇�ʂ̍����m�[�h���擾����B
			return Recursive_TransitionNode(transitionNode);
		}

		//�J�ڐ悪�Ȃ����߁APop���Ė߂�B
		return Recursive_TransitionNode(ReverseStack(node));
	}

	/// <summary>
	/// �����œn�����m�[�h�̎�O�m�[�h�܂ŃX�^�b�N��߂�
	/// </summary>
	/// <param name="targetNode">�߂肽���m�[�h</param>
	private void ReverseTargetBeforeStack(BehaviorNode targetNode)
	{
		if (m_currentStack.Count == 0) {
			return;
		}

		var currentNode = m_currentStack.Peek();
		if (currentNode == targetNode) {
			PopCurrentStack();  //�X�g�b�v������O�̃m�[�h�܂Ŗ߂��ďI��
			return;
		}

		PopCurrentStack();      //�X�^�b�N���|�b�v
		ReverseTargetBeforeStack(targetNode);   //�ċN����
	}

	/// <summary>
	/// �J�ڂ���Ƃ��̔��f�J�n�ʒu�̃m�[�h���擾����B
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
	/// �J�ڏ���
	/// </summary>
	private void Transition()
	{
		//�D��x�̈�ԍ����m�[�h�ɑJ��
		var node = Recursive_TransitionNode(GetTransitionStartNode());
		SetCurrentNode(node);   //�J�����g�m�[�h�̐ݒ�
	}

	/// <summary>
	/// �X�V����
	/// </summary>
	public void OnUpdate()
	{
		if (IsEmpty()) {
			return;
		}

		//�Ď����K�v�ȃf�R���[�^�̍X�V
		var stopNode = SearchStopNode();
		if (stopNode != null)
		{   //�߂�l�����݂���Ȃ�A�X�V�s�\�ȃm�[�h�����݂����B
			//�X�g�b�v�����m�[�h�̎�O�m�[�h�܂Ŗ߂�B
			ReverseTargetBeforeStack(stopNode);
			Transition();   //�J��
		}

		bool isEndTaskUpdate = TaskUpdate();    //�m�[�h�̍X�V

		//�m�[�h���I��������A�J��
		if (isEndTaskUpdate) {
			Transition();   //�J��
		}
	}
}







