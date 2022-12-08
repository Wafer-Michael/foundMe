﻿using System;
using System.Diagnostics.CodeAnalysis;

using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// BreadCrumb用のコンポーネント
/// </summary>
public class BreadCrumb : MonoBehaviour
{
    #region メンバ変数

    [SerializeField]
    private int m_numBread = 60;  //Breadの最大数

    [SerializeField]
    private float m_addRange = 1.0f;  //追加する距離

    private List<Vector3> m_positions = new List<Vector3>();

    #endregion

    #region Start,Update

    private void Start()
    {
        AddPosition();
    }

    private void Update()
    {
        //前回分より一定距離はなれたら
        if (IsAddRange())
        {
            AddPosition();

            if (IsSizeOver())
            {
                RemoveOldPosition();
            }
        }
    }

    #endregion

    #region 判断系関数

    private bool IsAddRange()
    {
        int index = m_positions.Count - 1;
        var beforePosition = m_positions[index];

        var toVec = beforePosition - transform.position;

        return toVec.magnitude > m_addRange ? true : false;
    }

    private bool IsSizeOver()
    {
        return m_positions.Count > m_numBread ? true : false;
    }

    #endregion

    #region private関数

    private void AddPosition()
    {
        m_positions.Add(transform.position);
    }

    /// <summary>
    /// 古いポジションの削除
    /// </summary>
    private void RemoveOldPosition()
    {
        m_positions.Remove(m_positions[0]);
    }

    #endregion

    #region アクセッサ

    /// <summary>
    /// 配列の値を参照渡し
    /// </summary>
    /// <returns>配列の参照</returns>
    public List<Vector3> GetPosisions()
    {
        return m_positions;
    }

    /// <summary>
    /// 配列の値をコピーして、取得
    /// </summary>
    /// <returns>配列を値渡し</returns>
    public List<Vector3> GetCopyPositions()
    {
        return new List<Vector3>(m_positions);
    }

    /// <summary>
    /// Breadの最大数をセット
    /// </summary>
    /// <param name="num">Breadの最大数</param>
    public void SetNumBread(int num)
    {
        m_numBread = num;
    }

    /// <summary>
    /// Breadの最大数を取得
    /// </summary>
    /// <returns>Breadの最大数</returns>
    public int GetNumBread()
    {
        return m_numBread;
    }

    /// <summary>
    /// 最新のポジションを取得する
    /// </summary>
    /// <returns></returns>
    public Vector3 GetNewPosition()
    {
        int index = m_positions.Count - 1;
        return m_positions[index];
    }

    /// <summary>
    /// 最新のポジションから引数の分前のポジションを返す。
    /// </summary>
    /// <param name="backCount">最新からどれだけ前のポジションか</param>
    /// <returns></returns>
    public Vector3? GetNewBackPosition(uint backCount)
    {
        backCount++;
        int index = m_positions.Count - (int)backCount;
        //インデックスが0より大きかったら返す。
        if(index >= 0){
            return m_positions[index];
        }
        else{
            return null;
        }
    }

    /// <summary>
    /// 次のポジションを取得する。
    /// </summary>
    /// <param name="beforePosition">前回分のポジション</param>
    /// <returns>次のポジション</returns>
    public Vector3? GetNextPosition(Vector3 beforePosition)
    {
        //最大の手前まで回す。
        for(int i = 0; i < m_positions.Count - 1; i++)
        {
            if(m_positions[i] == beforePosition)
            {
                return m_positions[++i];
            }
        }

        return null;
    }

    #endregion
}
