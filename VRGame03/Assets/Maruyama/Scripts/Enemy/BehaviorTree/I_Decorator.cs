using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface I_Decorator
{

	/// <summary>
	/// 生成時に一度だけ呼ぶ処理
	/// </summary>
	void OnCreate();

	/// <summary>
	/// ノード開始時に呼び出す処理。
	/// </summary>
	void OnStart();

	/// <summary>
	/// ノード終了時に呼び出す処理
	/// </summary>
	void OnExit();

	/// <summary>
	/// 遷移条件確認前の準備(CanTransitionを呼ぶ前に呼ぶ処理)
	/// </summary>
	void ReserveCanTransition();

	/// <summary>
	/// 遷移できるかどうか
	/// </summary>
	/// <returns>遷移できるならtrue</returns>
	bool CanTransition();

	/// <summary>
	/// アップデートが可能かどうか
	/// </summary>
	/// <returns>アップデートが可能ならtrue</returns>
	bool CanUpdate();
}
