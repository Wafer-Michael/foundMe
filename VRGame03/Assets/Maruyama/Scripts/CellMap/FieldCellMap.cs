using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FieldCellMap : FieldMapBase
{
    [Header("FloorObjectを設定した場合、widthCountとdepthCountは自動で設定される"),SerializeField]
    private Factory.CellMap.Parametor m_factoryParametor;   //セルマップ生成用のパラメータ

    private CellMap m_cellMap;  //セルマップ

    private void Awake()
    {
        m_cellMap = new CellMap();

        CreateCells();  //セルの生成
        CreateDebugDrawObjects();   //デバッグ表示
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
    /// デバッグ
    //--------------------------------------------------------------------------------------

    [SerializeField]
    private GameObject m_debugDrawPrefab;

    [SerializeField]
    private DebugDrawComponent.Parametor m_debugDrawParam = DebugDrawComponent.DEFAULT_PARAMETOR;

    private void CreateDebugDrawObjects()
    {
        if (m_debugDrawPrefab) {
            m_cellMap.CreateDebugDrawObjects(m_debugDrawPrefab, m_factoryParametor, m_debugDrawParam);
        }
    }
}
