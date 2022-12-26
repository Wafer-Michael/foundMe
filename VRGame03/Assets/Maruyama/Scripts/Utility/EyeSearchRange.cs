﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using System;


/// <summary>
/// ターゲットの管理用のパラメータ
/// </summary>
[Serializable]
public class EyeTargetParam
{
    public GameObject target;
    public bool isFind;  //見つけた状態かどうかを返す

    public EyeTargetParam(GameObject target)
    {
        this.target = target;
        this.isFind = false;
    }
}

/// <summary>
/// 視界のパラメータの構造体
/// </summary>
[Serializable]
public class EyeSearchRangeParam
{
    [Header("長さ")]
    public float range;  //索敵範囲(同心円状)
    [Header("高さ")]
    public float height; //索敵範囲(高さ)
    [Header("角度")]
    public float degree; //索敵範囲(角度)
    public float rad
    {
        get => degree * Mathf.Deg2Rad;
        set => degree = value * Mathf.Rad2Deg;
    }

    public EyeSearchRangeParam()
        :this(20.0f ,3.0f ,30.0f)
	{}

	public EyeSearchRangeParam(float range, float height, float degree)
    {
        this.range = range;
        this.height = height;
        this.degree = degree;
    }
}


/// <summary>
/// 視界の管理
/// </summary>
public class EyeSearchRange : MonoBehaviour
{
    #region メンバ変数

    //範囲に入っているかの対象になるオブジェクト
    [SerializeField]
    private List<EyeTargetParam> m_targetParams = new List<EyeTargetParam>();

    [SerializeField]
    private GameObject m_centerObject = null;

    [SerializeField]
    private EyeSearchRangeParam m_param = new EyeSearchRangeParam();

    /// <summary>
    /// Rayの障害物するLayerの配列
    /// </summary>
    [SerializeField]
    private string[] m_rayObstacleLayerStrings = new string[] { "L_Obstacle" };

    #endregion

    #region Awake, Update

    private void Awake()
    {
        //NullCheck
        if(m_centerObject == null)
        {
            m_centerObject = transform.gameObject;
        }
    }

    private void Update()
    {
        foreach(var param in m_targetParams)
        {
            if (IsInEyeRange(param.target))
            {  //ターゲットが視界に入っていたら。
                Hit(param);
            }
            else
            {
                param.isFind = false;
            }
        }
    }

    #endregion

    #region 判定用関数

    private bool IsRange(GameObject target) {
        return IsRange(target.transform.position);
	}

    private bool IsRange(Vector3 targetPosition) {
        var toVec = targetPosition - transform.position;
        //長さチェック
        return toVec.magnitude <= m_param.range ? true : false;
    }

    private bool IsHeight(GameObject target) {
        return IsHeight(target.transform.position);
	}

    private bool IsHeight(Vector3 targetPosition)
    {
        var selfPos = transform.position;

        var subHeight = targetPosition.y - selfPos.y;  //高さの差を求める。
        //高さが小さかったら。
        return Mathf.Abs(subHeight) <= m_param.height ? true : false;
    }

    private bool IsRad(GameObject target) {
        return IsRad(target.transform.position);
	}

    private bool IsRad(Vector3 targetPosition)
    {
        var forward = transform.transform.forward;
        forward.y = 0.0f;
        var toVec = targetPosition - transform.position;
        toVec.y = 0.0f;

        var newDot = Vector3.Dot(forward.normalized, toVec.normalized);
        var newRad = Mathf.Acos(newDot);
        //索敵範囲に入っていたら。
        return newRad <= m_param.rad ? true : false;
    }

    private bool IsRay(GameObject target){
        return IsRay(target.transform.position);
	}

    private bool IsRay(Vector3 targetPosition)
    {
        int obstacleLayer = LayerMask.GetMask(m_rayObstacleLayerStrings);
        var toVec = targetPosition - transform.position;

        const float SphereRange = 1.0f;
        var colliders = Physics.OverlapSphere(transform.position, SphereRange, obstacleLayer);  //オブジェクトに接触時に透けるバグ解消用
        //RaycastHit[] hits = Physics.SphereCastAll(m_centerObject.transform.position, SphereRange, toVec, obstacleLayer);
        RaycastHit hitData;
        var hit = Physics.Raycast(m_centerObject.transform.position, toVec, out hitData, 1000.0f, obstacleLayer);

        //Collider[] hits = Physics.OverlapSphere(playerTrans.position, 0.1f, layerMask, QueryTriggerInteraction.Collide);
        //RaycastHit[] hits2 = Physics.SphereCastAll(playerTrans.position, 0.1f, playerTrans.forward, 0.3f, layerMask, QueryTriggerInteraction.Collide);

        //if (!Physics.Linecast(m_centerObject.transform.position, targetPosition, obstacleLayer) && colliders.Length == 0)
        if (!hit && colliders.Length == 0)
        {
            return true;
        }

        if(colliders.Length != 0)
        {
            Debug.Log("重なってる");
        }

        return false;
    }

    #endregion

    private void Hit(EyeTargetParam targetParam) {
		targetParam.isFind = true;
	}

    #region IsInEyeRange

    /// <summary>
    /// 視界内にいるならtrueを返す
    /// </summary>
    /// <param name="targetPosition">ターゲットのポジション</param>
    /// <returns>視界の中にいるならtrue</returns>
    public bool IsInEyeRange(Vector3 targetPosition)
    {
        //全ての条件がtrueなら視界内に対象がいる。
        if (IsRange(targetPosition) && IsHeight(targetPosition) && IsRad(targetPosition) && IsRay(targetPosition))
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    /// <summary>
    /// 視界内にいるならtrueを返す
    /// </summary>
    /// <param name="target">ターゲット</param>
    /// <returns>視界の中にいるならtrue</returns>
    public bool IsInEyeRange(GameObject target)
    {
        if (target == null) { 
            return false; 
        }

        //全ての条件がtrueなら視界内に対象がいる。
        if (IsRange(target) && IsHeight(target) && IsRad(target) && IsRay(target)){
            return true;
        }
        else{
            return false;
        }
    }

    /// <summary>
    /// サーチ範囲を一時的に指定して、ターゲットの中に入っているかを判断
    /// </summary>
    /// <param name="target">ターゲット</param>
    /// <param name="range">索敵範囲</param>
    /// <returns>サーチ範囲ならtrue</returns>
    public bool IsInEyeRange(GameObject target, float range)
    {
        return IsInEyeRange(target.transform.position, range);
    }

    /// <summary>
    /// サーチ範囲を一時的に指定して、ターゲットの中に入っているかを判断
    /// </summary>
    /// <param name="target">ターゲット</param>
    /// <param name="range">索敵範囲</param>
    /// <returns>サーチ範囲ならtrue</returns>
    public bool IsInEyeRange(Vector3 targetPosition, float range)
    {
        float beforeRange = m_param.range;
        m_param.range = range;

        bool isRange = IsInEyeRange(targetPosition);
        m_param.range = beforeRange;

        return isRange;
    }

    /// <summary>
    /// サーチ範囲を一時的に指定して、ターゲットの中に入っているかを判断
    /// </summary>
    /// <param name="target">ターゲット</param>
    /// <param name="range">索敵範囲</param>
    /// <returns>サーチ範囲ならtrue</returns>
    public bool IsInEyeRange(GameObject target, EyeSearchRangeParam param)
    {
        var beforeParam = m_param;
        m_param = param;

        bool isRange = IsInEyeRange(target);
        m_param = beforeParam;

        return isRange;
    }

    #endregion

    //アクセッサ-------------------------------------------------------------------------

    public void AddTarget(GameObject target) {
		m_targetParams.Add(new EyeTargetParam(target));
	}

    public void SetParam(EyeSearchRangeParam param) {
		m_param = param;
	}

    public EyeSearchRangeParam GetParam() {
		return m_param;
	}

}
