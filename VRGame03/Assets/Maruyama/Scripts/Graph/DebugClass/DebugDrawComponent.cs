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
    public static readonly Parametor DEFAULT_PARAMETOR = new Parametor(DrawType.Cube, new Color(0.0f, 0.0f, 0.0f, 0.3f), 0.5f);

    public enum DrawType
    {
        Cube,
        Sphere,
    }

    [System.Serializable]
    public struct Parametor
    {
        public bool isSelectDraw;   //選択中に表示するかどうか
        public DrawType drawType;   //表示タイプ
        public Color color;         //色
        public float sphereRadius;  //スフィア表示時の半径

        public Parametor(
            DrawType drawType,
            Color color,
            float sphereRadius
        ) {
            this.drawType = drawType;
            this.color = color;
            this.sphereRadius = sphereRadius;
            isSelectDraw = false;
        }
    }

    #region メンバ変数

    public DrawType drawType {
        get => m_param.drawType;
        set => m_param.drawType = value;
    }

    public bool IsSelectDraw {
        get => m_param.isSelectDraw;
        set => m_param.isSelectDraw = value;
    }

    public Color GizmosColor {
        get => m_param.color;
        set => m_param.color = value;
    }

    public float SphereRadius {
        get => m_param.sphereRadius;
        set => m_param.sphereRadius = value;
    }

    [SerializeField]
    private Parametor m_param = new Parametor(DrawType.Cube, new Color(0.0f, 0.0f, 1.0f, 0.3f), 0.5f);  //パラメータ
    public Parametor Param  //パラメータのプロパティ
    {
        set => m_param = value;
        get => m_param;
    }

    #endregion

    #region Gizmos

    private void OnDrawGizmosSelected()
    {
        //セレクト時のみ表示だったら
        if (IsSelectDraw)
        {
            DrawGizmos();
        }
    }

    private void OnDrawGizmos()
    {
        //セレクト時のみ表示で無かったら
        if (!IsSelectDraw)
        {
            DrawGizmos();
        }
    }

    /// <summary>
    /// 生成範囲表示用
    /// </summary>
    private void DrawGizmos()
    {
        Gizmos.color = GizmosColor;

        Action drawFunc = drawType switch {
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
        Gizmos.DrawSphere(transform.position, SphereRadius);
    }

    #endregion
}
