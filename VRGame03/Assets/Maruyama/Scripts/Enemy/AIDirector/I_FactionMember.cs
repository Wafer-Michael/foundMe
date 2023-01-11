using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface I_FactionMember
{
    /// <summary>
    /// アサインするファクションを設定する。
    /// </summary>
    /// <param name="faction"></param>
    public void SetAssignedFaction(FactionCoordinator faction);

    /// <summary>
    /// アサインしているファクションを取得する。
    /// </summary>
    /// <returns></returns>
    public FactionCoordinator GetAssignedFaction();

    /// <summary>
    /// ファクションにアサインしたときに呼び出したい処理
    /// </summary>
    /// <param name="faction"></param>
    public virtual void AssignedFactionEvent(FactionCoordinator faction) { }

    /// <summary>
    /// ファクションから離脱したときに呼び出したい処理
    /// </summary>
    /// <param name="faction"></param>
    public virtual void UnsignedFactionEvent(FactionCoordinator faction) { }

    /// <summary>
    /// アサインするコーディネータを設定する。
    /// </summary>
    /// <param name="coordinator"></param>
    public void SetAssignedCoordinator(CoordinatorBase coordinator);

    /// <summary>
    /// アサインしているコーディネータを設定する。
    /// </summary>
    /// <returns></returns>
    public CoordinatorBase GetAssignedCoordinator();

    /// <summary>
    /// メンバーに加入したときに呼び出したいイベント
    /// </summary>
    public virtual void AssignCoordinatorEvent(CoordinatorBase coordinator) { }

    /// <summary>
    /// メンバーから離脱するときに呼び出したいイベント
    /// </summary>
    public virtual void UnsignCoordinatorEvent(CoordinatorBase coordinator) { }
}
