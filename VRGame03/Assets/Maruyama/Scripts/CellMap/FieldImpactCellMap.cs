using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FieldImpactCellMap : FieldMapBase
{
    [Header("FloorObject��ݒ肵���ꍇ�AwidthCount��depthCount�͎����Őݒ肳���"), SerializeField]
    private Factory.CellMap.Parametor m_factoryParametor;   //�Z���}�b�v�����p�̃p�����[�^

    private CellMap<ImpactCell> m_cellMap = new CellMap<ImpactCell>();  //�Z���}�b�v

    [SerializeField]
    private bool m_isDebug = true;

    private void Awake()
    {
        CreateCellMap();

        if (m_isDebug)
        {
            CreateDebugDrawObjects();   //�f�o�b�O�\��
        }
    }

    private void Update()
    {
        if (m_isDebug) {
            DebugColorUpdate(); //�f�o�b�O�\���̃J���[�X�V
        }
    }

    private void CreateCellMap()
    {
        m_cellMap = new CellMap<ImpactCell>();

        CreateCells();  //�Z���̐���
        m_cellMap.SetFieldData(new CellMapFieldData(m_factoryParametor.widthCount, m_factoryParametor.depthCount));
    }

    /// <summary>
    /// �Z���z��̐���
    /// </summary>
    private void CreateCells()
    {
        //���̐ݒ肪����Ȃ�A���ɍ��킹���p�����[�^�𐶐�����
        if (HasFloorObject())
        {
            SettingFactoryParametorForFloor();
        }

        var cells = Factory.CellMap.CreateCells<ImpactCell>(m_factoryParametor);

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

    public CellMap<ImpactCell> GetCellMap() { return m_cellMap; }

    //--------------------------------------------------------------------------------------
    /// �f�o�b�O
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
            //�Z���̊댯�x�ɍ��킹���F��\������
            var drawObject = debugDrawObjects[index];

            //�J���[�ݒ�
            //var alpha = m_debugDrawParam.color.a;   //���l�͕ς������Ȃ�����ۑ�
            //var color = m_debugDrawParam.color * cell.GetImpactData().dangerValue;  //�J���[���댯�x�ɍ��킹�ĕύX
            //color.a = alpha;    //���l�̐ݒ�

            float value = 1 - cell.GetImpactData().dangerValue;
            var color = new Color(1, value, value, m_debugDrawParam.color.a);

            drawObject.GizmosColor = color;         //�J���[�̃A�N�Z�b�T

            index++;    //�C���f�b�N�X�̍X�V
        }
    }
}
