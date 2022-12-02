using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//--------------------------------------------------------------------------------------
/// 監視対象が視界範囲にいるかどうかを判断するクラス
//--------------------------------------------------------------------------------------
public class ObserveIsInEyeTargets
{
    private List<GameObject> m_observeTargets = new List<GameObject>();  //監視対象

    private EyeSearchRange m_eyeRange;  //視界管理コンポーネント

    public ObserveIsInEyeTargets(List<GameObject> observeTargets, EyeSearchRange eyeRange) 
    {
        m_observeTargets = observeTargets;
        m_eyeRange = eyeRange;
    }

    public GameObject SearchIsInEyeTarget() {
        if(m_eyeRange == null) {    //視界管理がないなら処理をできない
            Debug.Log("EyeSearchRangeコンポーネントが存在しません。");
            return null;
        }

        foreach(var target in m_observeTargets) {
            if(target == null) {    //ターゲットが存在しないなら、処理を飛ばす。
                continue;
            }

            //視界範囲内なら、ターゲットを取得
            if (m_eyeRange.IsInEyeRange(target.transform.position)) {
                return target;
            }
        }

        return null;    //発見できなかったため、nullを返す。
    }

    public List<GameObject> SearchIsInEyeTargets() {
        var result = new List<GameObject>();

        if (m_eyeRange == null) {    //視界管理がないなら処理をできない
            Debug.Log("EyeSearchRangeコンポーネントが存在しません。");
            return result;
        }

        foreach(var target in m_observeTargets)
        {
            if(target == null) {
                continue;
            }

            //ターゲットが視界内なら、配列に入れる。
            if (m_eyeRange.IsInEyeRange(target.transform.position)) {
                result.Add(target);
            }
        }

        return result;
    }

    //--------------------------------------------------------------------------------------
    /// アクセッサ
    //--------------------------------------------------------------------------------------

    public void AddObserveTarget(GameObject target) { m_observeTargets.Add(target); }

    public void SetObserveTargets(List<GameObject> targets) { m_observeTargets = targets; }

    public List<GameObject> GetObserveTargets() { return m_observeTargets; }

    public void ClearObserveTargets() { m_observeTargets.Clear(); }
}
