using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FieldCellMap : FieldMapBase
{
    [Header("FloorObjectを設定した場合、widthCountとdepthCountは自動で設定される"),SerializeField]
    private Factory.CellMap.Parametor m_factoryParametor;   //セルマップ生成用のパラメータ

    private CellMap<Cell> m_cellMap;  //セルマップ

    [SerializeField]
    private bool m_isDebug = true;

    private void Awake()
    {
        CreateCellMap();

        if (m_isDebug) {
            CreateDebugDrawObjects();   //デバッグ表示
        }
    }
    private void CreateCellMap()
    {
        m_cellMap = new CellMap<Cell>();

        CreateCells();  //セルの生成
        m_cellMap.SetFieldData(new CellMapFieldData(m_factoryParametor.widthCount, m_factoryParametor.depthCount));
    }

    private void CreateCells()
    {
        //床の設定があるなら、床に合わせたパラメータを生成する
        if (HasFloorObject()) {
            SettingFactoryParametorForFloor();
        }

        var cells = Factory.CellMap.CreateCells(m_factoryParametor);

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

    public CellMap<Cell> GetCellMap() { return m_cellMap; }

    //--------------------------------------------------------------------------------------
    /// デバッグ
    //--------------------------------------------------------------------------------------

    [SerializeField]
    private DebugDrawComponent m_debugDrawPrefab;

    [SerializeField]
    private DebugDrawComponent.Parametor m_debugDrawParam = DebugDrawComponent.DEFAULT_PARAMETOR;

    private void CreateDebugDrawObjects()
    {
        if (m_debugDrawPrefab) {
            m_cellMap.CreateDebugDrawObjects(m_debugDrawPrefab, m_factoryParametor, m_debugDrawParam);
        }
    }
}
