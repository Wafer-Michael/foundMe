using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class BehaviorNode
{
	bool m_isActive = true;                         //アクティブ状態かどうか
	int m_index = 0;                                //ノードインデックス
	BehaviorState m_state = BehaviorState.Inactive; //ビヘイビアステート
	List<I_Decorator> m_decorators;                 //デコレータ配列

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
		{   //非アクティブなら遷移できないため、false
			return false;
		}

		//完了状態なら遷移しない。
		if (IsState(BehaviorState.Completed))
		{
			return false;
		}

		//デコレータがないなら、常にtrue
		if (IsDecoratorEmpty())
		{
			return true;
		}

		//一つでも遷移できないならfalse
		foreach (var decorator in m_decorators)
		{
			decorator.ReserveCanTransition();
			if (!decorator.CanTransition())
			{
				return false;
			}
		}

		return true;    //全てのデコレータがtrueなら遷移できる。
	}

	public bool CanUpdate()
	{
		if (IsDecoratorEmpty())
		{
			return true;
		}

		//一つでも更新できないならfalse
		foreach (var decorator in m_decorators)
		{
			if (!decorator.CanUpdate())
			{
				return false;
			}
		}

		return true;    //全てのデコレータがtrueなら更新できる。
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
