using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//--------------------------------------------------------------------------------------
/// �t�B�[���h�p�̃E�F�C�|�C���g�}�b�v
//--------------------------------------------------------------------------------------
public class FieldWayPointsMap : FieldMapBase
{
    [SerializeField]
    private Factory.WayPointsMap_FloodFill.Parametor m_factoryParametor;    //�E�F�C�|�C���g�����p�p�����[�^

        //�G���A�����}�b�v
    private WayPointsMap m_wayPointsMap;            //�E�F�C�|�C���g�}�b�v

    private void Awake()
    {
        m_wayPointsMap = new WayPointsMap();

        //�����m�[�h����
        if (HasFloorObject()) { //���ݒ肵�Ă���Ȃ�A����ɍ��킹��rect�𐶐�����B
            m_factoryParametor.rect = CalculateFloorRect();
        }
        m_wayPointsMap.CreateWayPointsMap(m_factoryParametor);

        //�O���t�̃f�o�b�O�\��
        CreateGraphDebugDraw();
    }

    private void Update()
    {
        //�f�o�b�O�������K�v�Ȃ�
        if (m_isDebugDraw) {
            UpdateGraphDebugDrawNode();
        }
    }

    //--------------------------------------------------------------------------------------
    /// �A�N�Z�b�T
    //--------------------------------------------------------------------------------------

    public WayPointsMap GetWayPointsMap() { return m_wayPointsMap; }

    public Factory.WayPointsMap_FloodFill.Parametor GetFactoryParametor() { return m_factoryParametor; }

    //--------------------------------------------------------------------------------------
    /// �f�o�b�O
    //--------------------------------------------------------------------------------------

    [SerializeField]
    private bool m_isDebugDraw = true;              //�f�o�b�O�\�����s�����ǂ���

    private DebugGraphDraw m_debugGraphDraw;        //�O���t�̃f�o�b�O�\���p

    [SerializeField]
    private DebugDrawComponent m_debugNodePrefab;           //�f�o�b�O�p�̃m�[�hPrefab

    [SerializeField]
    private float m_debugNodeScaleAdjust = 0.95f;   //�f�o�b�O�p�̃m�[�h�\���̑傫������(���������߂ɂ���Ƃ킩��₷��)

    //�m�[�h�̃f�o�b�O�\���p�̃p�����[�^
    [SerializeField]
    private DebugDrawComponent.Parametor m_debugNodeDrawParametor =
        new DebugDrawComponent.Parametor(DebugDrawComponent.DrawType.Sphere, new Color(0.0f, 0.0f, 1.0f, 0.3f), 0.5f);

    private void CreateGraphDebugDraw()
    {
        if (!m_isDebugDraw) {
            return;
        }

        //�O���t�̃f�o�b�O�\��
        var intervalRange = m_factoryParametor.intervalRange;
        var fScale = intervalRange * m_debugNodeScaleAdjust;    //�c���̃X�P�[���𒲐�
        var scale = new Vector3(fScale, 0.0f, fScale);
        m_debugGraphDraw = new DebugGraphDraw(this, m_wayPointsMap.GetGraph());
        m_debugGraphDraw.CreateDebugNodes(m_debugNodePrefab, scale, m_debugNodeDrawParametor);
        m_debugGraphDraw.CreateDebugEdges(m_debugNodePrefab, new Color(1.0f, 1.0f, 1.0f, 0.3f));
    }

    /// <summary>
    /// �f�o�b�O�\���̃m�[�h���X�V����B
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
            //�Z���̊댯�x�ɍ��킹���F��\������
            var draw = draws[index];

            float value = 1 - node.GetImpactData().dangerValue;
            var color = new Color(value, value, 1, m_debugNodeDrawParametor.color.a);

            draw.GizmosColor = color;   //�J���[�̃A�N�Z�b�T

            index++;    //�C���f�b�N�X�̍X�V
        }
    }
}
