using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FieldImpactCellMap : FieldMapBase
{
    [Header("FloorObjectを設定した場合、widthCountとdepthCountは自動で設定される"), SerializeField]
    private Factory.CellMap.Parametor m_factoryParametor;   //セルマップ生成用のパラメータ

    private CellMap<ImpactCell> m_cellMap = new CellMap<ImpactCell>();  //セルマップ

    [SerializeField]
    private bool m_isDebug = true;

    private void Awake()
    {
        CreateCellMap();

        if (m_isDebug)
        {
            CreateDebugDrawObjects();   //デバッグ表示
        }
    }

    private void Update()
    {
        if (m_isDebug) {
            DebugColorUpdate(); //デバッグ表示のカラー更新
        }
    }

    private void CreateCellMap()
    {
        m_cellMap = new CellMap<ImpactCell>();

        CreateCells();  //セルの生成
        m_cellMap.SetFieldData(new CellMapFieldData(m_factoryParametor.widthCount, m_factoryParametor.depthCount));
    }

    /// <summary>
    /// セル配列の生成
    /// </summary>
    private void CreateCells()
    {
        //床の設定があるなら、床に合わせたパラメータを生成する
        if (HasFloorObject())
        {
            SettingFactoryParametorForFloor();
        }

        var cells = Factory.CellMap.CreateCells<ImpactCell>(m_factoryParametor);

        m_cellMap.SetCells(cells);
    }

    /// <summary>
    /// 床に合わせたセルマップパラメータのセッティングをする。
    /// </summary>
    private void SettingFactoryParametorForFloor()
    {
        var floorRect = CalculateFloorRect();

        float widthCount = floorRect.width / m_factoryParametor.oneCellRect.width;
        float depthCount = floorRect.depth / m_factoryParametor.oneCellRect.depth;

        m_factoryParametor.widthCount = (int)widthCount;
        m_factoryParametor.depthCount = (int)depthCount;
    }

    //--------------------------------------------------------------------------------------
    /// アクセッサ
    //--------------------------------------------------------------------------------------

    public CellMap<ImpactCell> GetCellMap() { return m_cellMap; }

    //--------------------------------------------------------------------------------------
    /// デバッグ
    //--------------------------------------------------------------------------------------

    [SerializeField]
    private DebugDrawComponent m_debugDrawPrefab;

    [SerializeField]
    private DebugDrawComponent.Parametor m_debugDrawParam = DebugDrawComponent.DEFAULT_PARAMETOR;

    private void CreateDebugDrawObjects()
    {
        if (m_debugDrawPrefab)
        {
            m_cellMap.CreateDebugDrawObjects(m_debugDrawPrefab, m_factoryParametor, m_debugDrawParam);
        }
    }

    private void DebugColorUpdate()
    {
        int index = 0;
        var debugDrawObjects = m_cellMap.GetDebugDrawObjects();
        if(debugDrawObjects.Count == 0) {
            return;
        }

        foreach (var cell in m_cellMap.GetCells())
        {
            //セルの危険度に合わせた色を表示する
            var drawObject = debugDrawObjects[index];

            //カラー設定
            //var alpha = m_debugDrawParam.color.a;   //α値は変えたくないから保存
            //var color = m_debugDrawParam.color * cell.GetImpactData().dangerValue;  //カラーを危険度に合わせて変更
            //color.a = alpha;    //α値の設定

            float value = 1 - cell.GetImpactData().dangerValue;
            var color = new Color(1, value, value, m_debugDrawParam.color.a);

            drawObject.GizmosColor = color;         //カラーのアクセッサ

            index++;    //インデックスの更新
        }
    }
}
