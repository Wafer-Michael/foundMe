using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using System.Linq;

//--------------------------------------------------------------------------------------
/// �r�w�C�r�A�Z���N�^�[
//--------------------------------------------------------------------------------------
public class BehaviorSelecter : BehaviorNode
{
	//--------------------------------------------------------------------------------------
	/// �r�w�C�r�A�Z���N�^�[�̃Z���N�g�^�C�v
	//--------------------------------------------------------------------------------------
	public enum SelectType
	{
		Priority,   //�D��x
		Sequence,   //�V�[�P���X
		Random,     //�����_��
	};

	private BehaviorNode m_currentNode;             //���ݎg�p���̃m�[�h

	private SelectType m_selectType;                //�Z���N�g�^�C�v

	private BehaviorEdge m_fromEdge;                //��O�̃G�b�W
	private List<BehaviorEdge> m_transitionEdges;   //�J�ڐ�̃G�b�W�z��

	public BehaviorSelecter() :
		this(SelectType.Priority)
	{ }

	public BehaviorSelecter(SelectType selectType) {
		m_selectType = selectType;
	}

    public override void OnStart() { }

    public override bool OnUpdate() { return false; }

	public override void OnExit() {
		//�J�ڐ�̃G�b�W�����̏�Ԃɖ߂��B
		foreach (var edge in m_transitionEdges)
		{
			edge.GetToNode().SetState(BehaviorState.Inactive);
		}

		SetCurrentNode(null);    //�m�[�h��nullptr�ɕύX		
	}

	/// <summary>
	/// ���݂̃m�[�h����������
	/// </summary>
	/// <returns>���݂̃m�[�h</returns>
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
	/// �D��x�̈�ԍ����m�[�h���擾����
	/// </summary>
	/// <returns>�D��x�̈�ԍ����m�[�h</returns>
	private BehaviorNode SearchFirstPriorityNode()
	{
		//���݂̃X�e�[�g��Running�Ȃ�A��x���������Ă��邽�߁A�I���B
		if (IsState(BehaviorState.Running))
		{
			return null;
		}

		//�J�ڐ�m�[�h����Ȃ�nullptr
		if (IsEmptyTransitionNodes())
		{
			return null;
		}

		var transitionEdges = m_transitionEdges;       //�����o���\�[�g�����const�ɂł��Ȃ����߁A�ꎞ�ϐ���

		//�G�b�W�̗D��x�v�Z���ɂ���B
		foreach (var edge in transitionEdges)
		{
			if (edge == null)
			{   //���݂��Ȃ��Ȃ�
				continue;
			}

			edge.CalculatePriority();
		}

		//�����\�[�g
		transitionEdges.Sort();
		//std::sort(transitionEdges.begin(), transitionEdges.end(), &SortEdges);

		//���בւ����m�[�h���J�ڂł��邩�ǂ����𔻒f����B
		foreach (var edge in transitionEdges)
		{
			if (edge.GetToNode().CanTransition())
			{   //�J�ڂł���Ȃ�A���̃m�[�h��Ԃ��B
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
		{   //�J�ڐ悪���݂��Ȃ��Ȃ�nullptr��Ԃ��B
			return null;
		}

		var randomEdge = MaruUtility.MyRandom.RandomList(transitionEdges);
		return randomEdge.GetToNode();
	}

	private BehaviorNode SearchSequenceNode() {
		//�ς܂ꂽ�ォ�珇�ɑJ�ڂł���^�X�N�ɑJ�ځB
		foreach (var edge in m_transitionEdges) {
			var toNode = edge.GetToNode();
			if (toNode.CanTransition()) {
				return toNode;
			}
		}

		return null;
	}

	public void ChangeCurrentNode(BehaviorNode node) {
		//���݂̃m�[�h�̏I������������B
		var currentNode = GetCurrentNode();
		if (currentNode != null) {
			currentNode.OnExit();
		}

		m_currentNode = node;

		//�ύX����m�[�h��null�łȂ�������A�J�n����������B
		if (node != null)
		{
			node.OnStart();
		}
	}


	//--------------------------------------------------------------------------------------
	/// �A�N�Z�b�T
	//--------------------------------------------------------------------------------------

	public void SetSelectType(SelectType type) { m_selectType = type; }

	public SelectType GetSelectType() { return m_selectType; }

	/// <summary>
	/// ��O�G�b�W�̐ݒ�
	/// </summary>
	public void SetFromEdge(BehaviorEdge fromEdge) { m_fromEdge = fromEdge; }

	/// <summary>
	/// ��O�G�b�W�̎擾
	/// </summary>
	public BehaviorEdge GetFromEdge() { return m_fromEdge; }

	/// <summary>
	/// �J�ڐ�G�b�W�̒ǉ�
	/// </summary>
	public void AddTransitionEdge(BehaviorEdge edge) { m_transitionEdges.Add(edge); }

	/// <summary>
	/// �J�ڐ�̃m�[�h�����݂��邩�ǂ���
	/// </summary>
	/// <returns>���݂���Ȃ�true</returns>
	public bool IsEmptyTransitionNodes() { return m_transitionEdges.Count == 0; }

	/// <summary>
	/// �J�����g�m�[�h�����݂��邩�ǂ���
	/// </summary>
	public bool HasCurrentNode() { return m_currentNode != null; }

	/// <summary>
	/// ���ݐς܂�Ă���m�[�h��ݒ�
	/// </summary>
	/// <param name="node">�ς܂�Ă���m�[�h</param>
	public void SetCurrentNode(BehaviorNode node) { m_currentNode = node; }

	/// <summary>
	/// ���ݎg�p���̃m�[�h��Ԃ�
	/// </summary>
	public BehaviorNode GetCurrentNode() { return m_currentNode; }

}
