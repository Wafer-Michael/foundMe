using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class BehaviorNode
{
	bool m_isActive = true;                         //�A�N�e�B�u��Ԃ��ǂ���
	int m_index = 0;                                //�m�[�h�C���f�b�N�X
	BehaviorState m_state = BehaviorState.Inactive; //�r�w�C�r�A�X�e�[�g
	List<I_Decorator> m_decorators;                 //�f�R���[�^�z��

	public void OnDecoratorStart()
	{
		foreach (var decorator in m_decorators)
		{
			decorator.OnStart();
		}
	}

	public void OnDecoratorExit()
	{
		foreach (var decorator in m_decorators)
		{
			decorator.OnExit();
		}
	}

	public abstract void OnStart();
	public abstract bool OnUpdate();
	public abstract void OnExit();

	public void SetIsActive(bool isActive) { m_isActive = isActive; }

	public bool IsActive() { return m_isActive; }

	public void SetIndex(int index) { m_index = index; }

	public int GetIndex() { return m_index; }

	public void SetType<EnumType>(EnumType type) where EnumType : System.Enum { m_index = (int)(object)(type); }

	public EnumType GetType<EnumType>() where EnumType : System.Enum { return (EnumType)(object)m_index; }

	public void SetState(BehaviorState state) { m_state = state; }

	public BehaviorState GetState() { return m_state; }

	public bool IsState(BehaviorState state) { return m_state == state; }

	public bool CanTransition()
	{
		if (!IsActive())
		{   //��A�N�e�B�u�Ȃ�J�ڂł��Ȃ����߁Afalse
			return false;
		}

		//������ԂȂ�J�ڂ��Ȃ��B
		if (IsState(BehaviorState.Completed))
		{
			return false;
		}

		//�f�R���[�^���Ȃ��Ȃ�A���true
		if (IsDecoratorEmpty())
		{
			return true;
		}

		//��ł��J�ڂł��Ȃ��Ȃ�false
		foreach (var decorator in m_decorators)
		{
			decorator.ReserveCanTransition();
			if (!decorator.CanTransition())
			{
				return false;
			}
		}

		return true;    //�S�Ẵf�R���[�^��true�Ȃ�J�ڂł���B
	}

	public bool CanUpdate()
	{
		if (IsDecoratorEmpty())
		{
			return true;
		}

		//��ł��X�V�ł��Ȃ��Ȃ�false
		foreach (var decorator in m_decorators)
		{
			if (!decorator.CanUpdate())
			{
				return false;
			}
		}

		return true;    //�S�Ẵf�R���[�^��true�Ȃ�X�V�ł���B
	}

	public void AddDecorator(I_Decorator decorator) {
		m_decorators.Add(decorator);
	}

	public List<I_Decorator> GetDecorators() {
		return m_decorators;
	}

	public bool IsDecoratorEmpty() {
		return m_decorators.Count == 0;
	}

}
