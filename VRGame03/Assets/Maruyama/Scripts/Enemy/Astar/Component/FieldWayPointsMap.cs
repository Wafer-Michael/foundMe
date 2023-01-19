using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//--------------------------------------------------------------------------------------
/// フィールド用のウェイポイントマップ
//--------------------------------------------------------------------------------------
public class FieldWayPointsMap : FieldMapBase
{
    [SerializeField]
    private Factory.WayPointsMap_FloodFill.Parametor m_factoryParametor;    //ウェイポイント生成用パラメータ

        //エリア分けマップ
    private WayPointsMap m_wayPointsMap;            //ウェイポイントマップ

    private void Awake()
    {
        m_wayPointsMap = new WayPointsMap();

        //初期ノード生成
        if (HasFloorObject()) { //床設定しているなら、それに合わせたrectを生成する。
            m_factoryParametor.rect = CalculateFloorRect();
        }
        m_wayPointsMap.CreateWayPointsMap(m_factoryParametor);

        //グラフのデバッグ表示
        CreateGraphDebugDraw();
    }

    private void Update()
    {
        //デバッグ処理が必要なら
        if (m_isDebugDraw) {
            UpdateGraphDebugDrawNode();
        }
    }

    //--------------------------------------------------------------------------------------
    /// アクセッサ
    //--------------------------------------------------------------------------------------

    public WayPointsMap GetWayPointsMap() { return m_wayPointsMap; }

    //--------------------------------------------------------------------------------------
    /// デバッグ
    //--------------------------------------------------------------------------------------

    [SerializeField]
    private bool m_isDebugDraw = true;              //デバッグ表示を行うかどうか

    private DebugGraphDraw m_debugGraphDraw;        //グラフのデバッグ表示用

    [SerializeField]
    private DebugDrawComponent m_debugNodePrefab;           //デバッグ用のノードPrefab

    [SerializeField]
    private float m_debugNodeScaleAdjust = 0.95f;   //デバッグ用のノード表示の大きさ調整(少し小さめにするとわかりやすい)

    //ノードのデバッグ表示用のパラメータ
    [SerializeField]
    private DebugDrawComponent.Parametor m_debugNodeDrawParametor =
        new DebugDrawComponent.Parametor(DebugDrawComponent.DrawType.Sphere, new Color(0.0f, 0.0f, 1.0f, 0.3f), 0.5f);

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
        m_debugGraphDraw.CreateDebugNodes(m_debugNodePrefab, scale, m_debugNodeDrawParametor);
        m_debugGraphDraw.CreateDebugEdges(m_debugNodePrefab, new Color(1.0f, 1.0f, 1.0f, 0.3f));
    }

    /// <summary>
    /// デバッグ表示のノードを更新する。
    /// </summary>
    private void UpdateGraphDebugDrawNode()
    {
        var nodes = m_wayPointsMap.GetGraph().GetNodes();
        var draws = m_debugGraphDraw.GetNodes();
        if(draws.Count == 0) {
            return;
        }

        int index = 0;
        foreach (var node in nodes)
        {
            //セルの危険度に合わせた色を表示する
            var draw = draws[index];

            float value = 1 - node.GetImpactData().dangerValue;
            var color = new Color(value, value, 1, m_debugNodeDrawParametor.color.a);

            draw.GizmosColor = color;   //カラーのアクセッサ

            index++;    //インデックスの更新
        }
    }
}
