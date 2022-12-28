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
    public enum DrawType
    {
        Cube,
        Sphere,
    }


    #region メンバ変数

    [SerializeField]
    private DrawType m_drawType = DrawType.Cube;
    public DrawType drawType {
        get => m_drawType;
        set => m_drawType = value;
    }

    [Header("セレクト時のみ範囲を表示するかどうか"),SerializeField]
    private bool m_isSelectDrawGizmos = false;
    public bool IsSelectDrawGizmos {
        get => m_isSelectDrawGizmos;
        set => m_isSelectDrawGizmos = value;
    }

    [Header("生成範囲表示カラー"),SerializeField]
    private Color m_gizmosColor = new Color(1.0f, 0, 0, 0.3f);
    public Color GizmosColor {
        get => m_gizmosColor;
        set => m_gizmosColor = value;
    }

    [SerializeField]
    protected float m_sphereRadius = 0.5f;          //スフィアの半径
    public float SphereRadius {
        get => m_sphereRadius;
        set => m_sphereRadius = value;
    }


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
        Gizmos.matrix = Matrix4x4.TRS(transform.position, transform.rotation, transform.lossyScale);    //マトリックス設定
        Gizmos.DrawCube(Vector3.zero, Vector3.one);
    }

    private void SphereDraw()
    {   
        Gizmos.DrawSphere(transform.position, m_sphereRadius);
    }

    #endregion
}
