using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//--------------------------------------------------------------------------------------
/// フィールド用のウェイポイントマップ
//--------------------------------------------------------------------------------------
public class FieldWayPointsMap : MonoBehaviour
{
    [SerializeField]
    private GameObject m_floorObject;       //ウェイポイントを展開したい床オブジェクト
    private bool m_isPlane = true;          //床オブジェクトがプレーンかどうか

    [SerializeField]
    private Factory.WayPointsMap_FloodFill.Parametor m_factoryParametor;    //ウェイポイント生成用パラメータ

    private WayPointsMap m_wayPointsMap;    //ウェイポイントマップ

    private void Awake()
    {
        m_wayPointsMap = new WayPointsMap();

        //初期ノード生成
        m_factoryParametor.rect = CalculateFieldRect();
        m_wayPointsMap.CreateWayPointsMap(m_factoryParametor);
    }

    private void Update()
    {
        foreach(var node in m_wayPointsMap.GetGraph().GetNodes())
        {
            
        }
    }

    /// <summary>
    /// フィールド用の四角範囲データを計算
    /// </summary>
    /// <returns></returns>
    private maru.Rect CalculateFieldRect() 
    {
        //床オブジェクトの設定がしてあるなら、床に合わせたrectを生成
        if (m_floorObject)
        {
            var rect = new maru.Rect();
            rect.centerPosition = m_floorObject.transform.position;
            rect.width = m_floorObject.transform.localScale.x * GetFloorScaleAdjust();
            rect.depth = m_floorObject.transform.localScale.z * GetFloorScaleAdjust();
            return rect;
        }

        return m_factoryParametor.rect; //そうでないなら、Serializeで設定した大きさ
    }

    /// <summary>
    /// 床データのスケールの調整(planeかboxで全然違う大きさだから)
    /// </summary>
    /// <returns></returns>
    private float GetFloorScaleAdjust() { return m_isPlane ? 10 : 1; }
}
