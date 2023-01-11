using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FieldMapBase : MonoBehaviour
{
    [SerializeField]
    private GameObject m_floorObject;   //マップを展開する床オブジェクト
    protected GameObject GetFloorObject() => m_floorObject;
    protected bool HasFloorObject() => m_floorObject != null;

    [SerializeField]
    private bool m_isPlane = true;
    protected bool IsPlane => m_isPlane;

    /// <summary>
    /// フィールド用の四角範囲データを計算
    /// </summary>
    /// <returns></returns>
    protected maru.Rect CalculateFloorRect()
    {
        var rect = new maru.Rect();

        //床オブジェクトの設定がしてあるなら、床に合わせたrectを生成
        if (m_floorObject)
        {
            rect.centerPosition = m_floorObject.transform.position;
            rect.width = m_floorObject.transform.localScale.x * GetFloorScaleAdjust();
            rect.depth = m_floorObject.transform.localScale.z * GetFloorScaleAdjust();
        }

        return rect;
    }

    /// <summary>
    /// 床データのスケールの調整(planeかboxで全然違う大きさだから)
    /// </summary>
    /// <returns></returns>
    private float GetFloorScaleAdjust() { return m_isPlane ? 10 : 1; }
}
