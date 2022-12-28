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

    private WayPointsMap m_wayPointsMap;        //ウェイポイントマップ

    [SerializeField]
    private bool m_isDebugDraw = true;

    private DebugGraphDraw m_debugGraphDraw;    //グラフのデバッグ表示用
    [SerializeField]
    private GameObject m_debugNodePrefab;       //デバッグ用のノードPrefab
    [SerializeField]
    private float m_debugNodeScaleAdjust = 0.95f;   //デバッグ用のノード表示の大きさ調整(少し小さめにするとわかりやすい)

    private void Awake()
    {
        m_wayPointsMap = new WayPointsMap();

        //初期ノード生成
        m_factoryParametor.rect = CalculateFieldRect();
        m_wayPointsMap.CreateWayPointsMap(m_factoryParametor);

        //グラフのデバッグ表示
        CreateGraphDebugDraw();
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


    //--------------------------------------------------------------------------------------
    /// デバッグ
    //--------------------------------------------------------------------------------------

    private void CreateGraphDebugDraw()
    {
        if (!m_isDebugDraw) {
            return;
        }

        //グラフのデバッグ表示
        var intervalRange = m_factoryParametor.intervalRange;
        var fScale = intervalRange * m_debugNodeScaleAdjust;    //縦横のスケールを調整
        var scale = new Vector3(fScale, 0.0f, fScale);
        m_debugGraphDraw = new DebugGraphDraw(this, m_wayPointsMap.GetGraph());
        m_debugGraphDraw.CreateDebugNodes(m_debugNodePrefab, scale, DebugDrawComponent.DrawType.Sphere);
        m_debugGraphDraw.CreateDebugEdges(m_debugNodePrefab, new Color(1.0f, 1.0f, 1.0f, 0.3f));
    }
}
