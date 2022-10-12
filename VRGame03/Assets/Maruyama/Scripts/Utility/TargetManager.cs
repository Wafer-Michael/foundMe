using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// ターゲットを見失ったときに生成するデータ
/// </summary>
public struct TargetLostData
{
    
}

/// <summary>
/// ターゲットのデータ
/// </summary>
public struct TargetData
{
    public GameObject target;       //ターゲットのゲームオブジェクト
    public float priority;          //優先度
    public TargetLostData lostData; //見失ったときのデータ

    public TargetData(GameObject target)
    {
        this.target = target;
        this.priority = 0;
        this.lostData = new TargetLostData();
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
}
