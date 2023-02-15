using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cell : I_GraphNode
{
	//--------------------------------------------------------------------------------------
	///	�Z���p�����[�^
	//--------------------------------------------------------------------------------------
	public struct Parametor
	{
		public maru.Rect rect;

		public Parametor(maru.Rect rect) {
			this.rect = rect;
        }
	}

	private bool m_isActive;			//�A�N�e�B�u���

	private bool m_isTarget = false;	//�^�[�Q�b�g�Ґ�

	private int m_index;				//�����̃C���f�b�N�X

	private Parametor m_param;			//�p�����[�^

	public Cell(int index, Parametor param)
    {
		m_isActive = true;
		m_index = index;
		m_param = param;
	}

	//--------------------------------------------------------------------------------------
	///	�A�N�Z�b�T
	//--------------------------------------------------------------------------------------

	public void SetPosition(Vector3 position) { m_param.rect.centerPosition = position; }

	public Vector3 GetPosition() { return m_param.rect.centerPosition; }

	public void SetWidth(float width) { m_param.rect.width = width; }

	public float GetWidth() { return m_param.rect.width; }

	public void SetDepth(float depth) { m_param.rect.depth = depth; }

	public float GetDepth() { return m_param.rect.depth; }

	public void SetParametor(Parametor parametor) { m_param = parametor; }

	public Parametor GetParametor() { return m_param; }

	public void SetRectData(maru.Rect rect) { m_param.rect = rect; }

	public maru.Rect GetRectData() { return m_param.rect; }

	public void SetIndex(int index) { m_index = index; }

	public int GetIndex() { return m_index; }

	public void SetIsActive(bool isActive) { m_isActive = isActive; }

	public bool IsActive() { return m_isActive; }

	public bool IsTarget
    {
		get => m_isTarget;
		set => m_isTarget = value;
    }

}