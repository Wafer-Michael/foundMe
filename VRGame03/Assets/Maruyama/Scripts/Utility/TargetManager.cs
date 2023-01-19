using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// ターゲットを見失ったときに生成するデータ
/// </summary>
public struct TargetLostData
{
    public GameObject target;       //見失った相手
    public Vector3 lostPosition;    //見失った場所

    public TargetLostData(GameObject target, Vector3 lostPosition)
    {
        this.target = target;
        this.lostPosition = lostPosition;
    }
}

/// <summary>
/// ターゲットのデータ
/// </summary>
public struct TargetData
{
    public GameObject target;       //ターゲットのゲームオブジェクト
    public float priority;          //優先度
    public TargetLostData lostData; //見失ったときのデータ
    public Targeted targeted;

    public TargetData(GameObject target)
    {
        this.target = target;
        this.priority = 0;
        this.lostData = new TargetLostData();
        targeted = target ? target.GetComponent<Targeted>() : null;
    }
}

/// <summary>
/// ターゲット管理のコンポーネント
/// </summary>
public class TargetManager : MonoBehaviour
{
    public TargetData m_currentData;    //現在のターゲット

    /// <summary>
    /// ターゲットを持っているかどうか
    /// </summary>
    /// <returns>持っているならtrue</returns>
    public bool HasTarget()
    {
        return m_currentData.target != null ? true : false;
    }

    /// <summary>
    /// 現在のターゲットを設定する。
    /// </summary>
    /// <param name="target">ターゲット</param>
    public void SetCurrentTarget(GameObject target)
    {
        m_currentData = new TargetData(target);
    }

    public GameObject GetCurrentTarget()
    {
        return m_currentData.target;
    }

    public Vector3 CalculateSelfToTargetVector()
    {
        return GetCurrentTarget().transform.position - transform.position;
    }

    /// <summary>
    /// ターゲットを見失った場所を記録する。
    /// </summary>
    /// <returns>ターゲットを見失った場所</returns>
    public Vector3 GetLostTargetPosition() { return m_currentData.lostData.lostPosition; }

    /// <summary>
    /// 見失った場所への方向ベクトルを取得
    /// </summary>
    /// <returns>見失った場所への方向ベクトル</returns>
    public Vector3 CalculateSelfLostPositionVector() { return GetLostTargetPosition() - transform.position; }

    /// <summary>
    /// 対象が視界から外れたことを伝える。
    /// </summary>
    public void CallLostTarget()
    {
        //ターゲットを持っていなかったら処理が不可能
        if (!HasTarget()) {
            return;
        }

        var target = GetCurrentTarget();
        m_currentData.lostData = new TargetLostData(target, target.transform.position);
    }
}
