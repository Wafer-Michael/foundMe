using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using System;
using MaruUtility;

#region ターゲットデータ

/// <summary>
/// 一定距離離れた場所に生成したいデータ構造体
/// </summary>
[Serializable]
public struct OutOfTargetData
{
    public GameObject target;  //ターゲット
    public float range;  //どれだけ離れた距離か

    public OutOfTargetData(GameObject target)
        :this(target, 15.0f)
    {}

    public OutOfTargetData(GameObject target, float range)
    {
        this.target = target;
        this.range = range;
    }
}

#endregion

public class DebugDrawComponent : MonoBehaviour
{
    enum DrawType
    {
        Cube,
        Sphere,
    }


    #region メンバ変数

    [SerializeField]
    DrawType m_drawType = DrawType.Cube;

    [Header("セレクト時のみ範囲を表示するかどうか"),SerializeField]
    private bool m_isSelectDrawGizmos = false;
    [Header("生成範囲表示カラー"),SerializeField]
    private Color m_gizmosColor = new Color(1.0f, 0, 0, 0.3f);

    [SerializeField]
    protected Vector3 m_cubeSize = new Vector3();   //Gizmoの生成サイズ

    [SerializeField]
    protected float m_sphereRadius = 0.5f;          //スフィアの半径


    #endregion

    #region Gizmos

    private void OnDrawGizmosSelected()
    {
        //セレクト時のみ表示だったら
        if (m_isSelectDrawGizmos)
        {
            DrawGizmos();
        }
    }

    private void OnDrawGizmos()
    {
        //セレクト時のみ表示で無かったら
        if (!m_isSelectDrawGizmos)
        {
            DrawGizmos();
        }
    }

    /// <summary>
    /// 生成範囲表示用
    /// </summary>
    private void DrawGizmos()
    {
        Gizmos.color = m_gizmosColor;

        Action drawFunc = m_drawType switch {
            DrawType.Cube => CubeDraw,
            DrawType.Sphere => SphereDraw,
            _ => null
        };

        drawFunc?.Invoke();
    }

    private void CubeDraw()
    {
        var cubeSize = m_cubeSize * 2.0f;
        Gizmos.DrawCube(transform.position, cubeSize);
    }

    private void SphereDraw()
    {
        Gizmos.DrawSphere(transform.position, m_sphereRadius);
    }

    #endregion
}
