using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BehaviorEdge
{
	private BehaviorNode m_fromNode;   //�����̎�O�̃m�[�h
	private BehaviorNode m_toNode;     //�����̐�̃m�[�h

	private float m_priority;          //�D��x

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
	/// ��Ԃ̃m�[�h��ݒ�
	/// </summary>
	/// <param name="node">��O�̃m�[�h</param>
	public void SetFromNode(BehaviorNode node) { m_fromNode = node; }

	/// <summary>
	/// ��O�̃m�[�h���擾
	/// </summary>
	/// <returns>��O�̃m�[�h</returns>
	public BehaviorNode GetFromNode() { return m_fromNode; }

	/// <summary>
	/// ��̃m�[�h�̐ݒ�
	/// </summary>
	/// <param name="node">��̃m�[�h</param>
	public void SetToNode(BehaviorNode node) { m_toNode = node; }

	/// <summary>
	/// ��̃m�[�h���擾
	/// </summary>
	/// <returns>��̃m�[�h</returns>
	public BehaviorNode GetToNode() { return m_toNode; }

	/// <summary>
	/// �D��x�̐ݒ�
	/// </summary>
	/// <param name="priority">�D��x</param>
	public void SetPriority(float priority) { m_priority = priority; }

	/// <summary>
	/// �D��x�̎擾
	/// </summary>
	/// <returns>�D��x</returns>
	public float GetPriority() { return m_priority; }

	/// <summary>
	/// �D��x�̌v�Z
	/// </summary>
	/// <returns>�v�Z��̗D��x</returns>
	public float CalculatePriority() {
		//�����I�Ɍv�Z����������
		//���݂͂��̂܂܂̗D��x��Ԃ��B

		return GetPriority();
	}
}
