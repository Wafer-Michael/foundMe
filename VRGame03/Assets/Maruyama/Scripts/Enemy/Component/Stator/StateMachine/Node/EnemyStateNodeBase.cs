﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using System;

#region パラメータ

//コンポーネントの変更関係の情報をまとめた構造体
[Serializable]
public class ChangeCompParam
{
	public Behaviour behaviour;
	public bool isStart;  //スタート時にどっちにするか？
	public bool isExit;   //終了時にどっちにするか？

	public ChangeCompParam(Behaviour behaviour, bool isStart, bool isExit)
	{
		this.behaviour = behaviour;
		this.isStart = isStart;
		this.isExit = isExit;
	}
}

#endregion

/// <summary>
/// Enemy用の基底StateNodeクラス
/// </summary>
/// <typeparam name="EnemyType">BaseEnemyの子クラス</typeparam>
public abstract class EnemyStateNodeBase<EnemyType> : NodeBase<EnemyType>
    where EnemyType : class
{
	protected enum EnableChangeType {
		Start,
		Exit,
	}

	private List<ChangeCompParam> m_changeParams;

    #region コンストラクタ
    public EnemyStateNodeBase(EnemyType enemy)
        :base(enemy)
    {
		m_changeParams = new List<ChangeCompParam>();
	}
    #endregion

    #region Start,Exit

    public override void OnStart()
    {
		ReserveChangeComponents();

		ChangeComps(EnableChangeType.Start);
    }

    public override void OnExit()
    {
		ChangeComps(EnableChangeType.Exit);
    }

	#endregion

	#region protected関数

	/// <summary>
	/// 切り替えるコンポーネントの準備
	/// </summary>
	protected virtual void ReserveChangeComponents() { }

	/// <summary>
	/// 開始と終了時に切り替えるコンポーネントの追加
	/// </summary>
	/// <param name="behaviour">切り替えるコンポーネントのポインタ</param>
	/// <param name="isStart">スタート時にどっちに切り替える</param>
	/// <param name="isExit">終了時にどっちに切り替えるか</param>
	protected void AddChangeComp(Behaviour behaviour, bool isStart, bool isExit)
	{
		if (behaviour == null) {  //nullptrなら追加しない
			return;
		}

		var param = new ChangeCompParam(behaviour, isStart, isExit);
		m_changeParams.Add(param);
	}

	/// <summary>
	/// 登録されたコンポーネントの切り替えを行う
	/// </summary>
	/// <param name="type">StartかExitの切替タイプ</param>
	protected void ChangeComps(EnableChangeType type)
	{
		foreach(var param in m_changeParams)
        {
			bool isEnable = type switch
            {
				EnableChangeType.Start => param.isStart,
				EnableChangeType.Exit => param.isExit,
				_ => false
			};

            param.behaviour.enabled = isEnable;
		}
	}

    #endregion
}
