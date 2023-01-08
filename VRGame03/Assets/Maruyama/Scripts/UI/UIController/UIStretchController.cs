using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Oculus.Interaction;

/// <summary>
/// UIを引き延ばしたりする処理
/// </summary>
public class UIStretchController : MonoBehaviour
{
    /// <summary>
    /// デフォルトパラメータ
    /// </summary>
    public static readonly Parametor DEFAULT_PARAMETOR = new Parametor()
    {
        minSizeRatio = 0.25f,
        maxSizeRatio = 1.0f,
        stretchType = StretchType.Horizontal
    };

    /// <summary>
    /// 引き延ばす方向タイプ
    /// </summary>
    public enum StretchType { 
        Horizontal, //横方向
        Vertical,   //縦方向
    }

    [System.Serializable]
    public struct Parametor
    {
        public float minSizeRatio;      //最小サイズの割合
        public float maxSizeRatio;      //最大サイズの割合
        public StretchType stretchType; //引き延ばしたい方向タイプ
    }

    [SerializeField]
    private Parametor m_param = DEFAULT_PARAMETOR;  //パラメータ

    private Vector3 m_initializePosition;           //初期位置

    [SerializeField]
    private SpriteRenderer m_spriteRender;          //スプライトレンダー

    [SerializeField]
    private BoxProximityField m_boxProximityField;  //UIの当たり判定フィールド

    private void Awake()
    {
        if (IsNullFaild()) {   //スプライトレンダーが存在しないなら処理をしない。
            Debug.Log("UIStretchController::Awake(): SpriteRenderがnullです。");
            return;
        }

        m_initializePosition = m_spriteRender.transform.position;   //初期位置設定
    }

    public void Tocuch_Mover(PointerEvent pointer)
    {
        if (IsNullFaild()) {   //スプライトレンダーが存在しないなら処理をしない。
            return;
        }

        var size = CalculateSize(pointer);
        //Debug.Log("★" + size.ToString());
        m_spriteRender.size = size;
    }

    /// <summary>
    /// サイズを取得する。
    /// </summary>
    /// <param name="pointer">タッチポインター</param>
    /// <returns></returns>
    private Vector2 CalculateSize(in PointerEvent pointer)
    {
        //var range = CalculateInitializeToCurrentRange(pointer);
        var ratio = CalculatePositionRatio(pointer);
        var clampRatio = Mathf.Clamp(ratio, MinSizeRatio, MaxSizeRatio);
        Debug.Log("clampRatio: " + m_boxProximityField.transform.localScale.x.ToString());
        var range = clampRatio * m_boxProximityField.transform.localScale.x;
        var currentSize = m_spriteRender.size;  //現在のサイズ

        Vector2 size = GetStretchType() switch
        {
            StretchType.Horizontal => new Vector2(range, currentSize.y),
            StretchType.Vertical => new Vector2(currentSize.x, range),
            _ => m_spriteRender.size
        };

        return size;
    }

    /// <summary>
    /// 初期位置から現在の位置までの距離を計算して返す。
    /// </summary>
    /// <returns>初期位置から現在の位置までの距離</returns>
    private float CalculateInitializeToCurrentRange(in PointerEvent pointer)
    {
        var toInitializeVec = m_initializePosition - pointer.Pose.position;
        return toInitializeVec.magnitude;
    }

    /// <summary>
    /// 現在タッチした位置が、どの位置にいるかを割合で返す。
    /// </summary>
    /// <returns></returns>
    private float CalculatePositionRatio(in PointerEvent pointer)
    {
        var scale = m_boxProximityField.transform.lossyScale;
        var startPosition = CalculateFieldLeftPosition();
        var range = pointer.Pose.position.x - startPosition.x;

        return range / scale.x;
    }

    /// <summary>
    /// フィールドの左端の位置を取得
    /// </summary>
    /// <returns></returns>
    private Vector3 CalculateFieldLeftPosition()
    {
        var position = m_boxProximityField.transform.position;
        var halfSize = m_boxProximityField.transform.lossyScale.x * 0.5f;
        return position + -Vector3.right * halfSize;
    }

    //--------------------------------------------------------------------------------------
    /// アクセッサ
    //--------------------------------------------------------------------------------------

    public float MinSizeRatio
    {
        set => m_param.minSizeRatio = value;
        get => m_param.minSizeRatio;
    }

    public float MaxSizeRatio
    {
        set => m_param.maxSizeRatio = value;
        get => m_param.maxSizeRatio;
    }

    public void SetStretchType(StretchType type) { m_param.stretchType = type; }

    public StretchType GetStretchType() { return m_param.stretchType; }

    public Vector3 InitializePosition => m_initializePosition;

    /// <summary>
    /// 特定のオブジェクトがnullかどうか(nullなら処理を省く)
    /// </summary>
    /// <returns></returns>
    public bool IsNullFaild() {
        return !m_spriteRender || !m_boxProximityField;
    }
}
