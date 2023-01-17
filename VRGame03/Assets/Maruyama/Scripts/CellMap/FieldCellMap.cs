using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FieldCellMap : FieldMapBase
{
    [Header("FloorObject��ݒ肵���ꍇ�AwidthCount��depthCount�͎����Őݒ肳���"),SerializeField]
    private Factory.CellMap.Parametor m_factoryParametor;   //�Z���}�b�v�����p�̃p�����[�^

    private CellMap<Cell> m_cellMap;  //�Z���}�b�v

    [SerializeField]
    private bool m_isDebug = true;

    private void Awake()
    {
        CreateCellMap();

        if (m_isDebug) {
            CreateDebugDrawObjects();   //�f�o�b�O�\��
        }
    }
    private void CreateCellMap()
    {
        m_cellMap = new CellMap<Cell>();

        CreateCells();  //�Z���̐���
        m_cellMap.SetFieldData(new CellMapFieldData(m_factoryParametor.widthCount, m_factoryParametor.depthCount));
    }

    private void CreateCells()
    {
        //���̐ݒ肪����Ȃ�A���ɍ��킹���p�����[�^�𐶐�����
        if (HasFloorObject()) {
            SettingFactoryParametorForFloor();
        }

        var cells = Factory.CellMap.CreateCells(m_factoryParametor);

        m_cellMap.SetCells(cells);
    }

    /// <summary>
    /// ���ɍ��킹���Z���}�b�v�p�����[�^�̃Z�b�e�B���O������B
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
    /// �A�N�Z�b�T
    //--------------------------------------------------------------------------------------

    public CellMap<Cell> GetCellMap() { return m_cellMap; }

    //--------------------------------------------------------------------------------------
    /// �f�o�b�O
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
