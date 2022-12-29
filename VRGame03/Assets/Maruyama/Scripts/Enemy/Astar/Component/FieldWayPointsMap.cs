using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//--------------------------------------------------------------------------------------
/// �t�B�[���h�p�̃E�F�C�|�C���g�}�b�v
//--------------------------------------------------------------------------------------
public class FieldWayPointsMap : MonoBehaviour
{
    [SerializeField]
    private GameObject m_floorObject;       //�E�F�C�|�C���g��W�J���������I�u�W�F�N�g
    private bool m_isPlane = true;          //���I�u�W�F�N�g���v���[�����ǂ���

    [SerializeField]
    private Factory.WayPointsMap_FloodFill.Parametor m_factoryParametor;    //�E�F�C�|�C���g�����p�p�����[�^

    private WayPointsMap m_wayPointsMap;        //�E�F�C�|�C���g�}�b�v

    [SerializeField]
    private bool m_isDebugDraw = true;

    private DebugGraphDraw m_debugGraphDraw;        //�O���t�̃f�o�b�O�\���p

    [SerializeField]
    private GameObject m_debugNodePrefab;           //�f�o�b�O�p�̃m�[�hPrefab

    [SerializeField]
    private float m_debugNodeScaleAdjust = 0.95f;   //�f�o�b�O�p�̃m�[�h�\���̑傫������(���������߂ɂ���Ƃ킩��₷��)

    //�m�[�h�̃f�o�b�O�\���p�̃p�����[�^
    [SerializeField]
    private DebugDrawComponent.Parametor m_debugNodeDrawParametor =
    new DebugDrawComponent.Parametor(DebugDrawComponent.DrawType.Sphere, new Color(0.0f, 0.0f, 1.0f, 0.3f), 0.5f);

    private void Awake()
    {
        m_wayPointsMap = new WayPointsMap();

        //�����m�[�h����
        m_factoryParametor.rect = CalculateFieldRect();
        m_wayPointsMap.CreateWayPointsMap(m_factoryParametor);

        //�O���t�̃f�o�b�O�\��
        CreateGraphDebugDraw();
    }

    /// <summary>
    /// �t�B�[���h�p�̎l�p�͈̓f�[�^���v�Z
    /// </summary>
    /// <returns></returns>
    private maru.Rect CalculateFieldRect() 
    {
        //���I�u�W�F�N�g�̐ݒ肪���Ă���Ȃ�A���ɍ��킹��rect�𐶐�
        if (m_floorObject)
        {
            var rect = new maru.Rect();
            rect.centerPosition = m_floorObject.transform.position;
            rect.width = m_floorObject.transform.localScale.x * GetFloorScaleAdjust();
            rect.depth = m_floorObject.transform.localScale.z * GetFloorScaleAdjust();
            return rect;
        }

        return m_factoryParametor.rect; //�����łȂ��Ȃ�ASerialize�Őݒ肵���傫��
    }

    /// <summary>
    /// ���f�[�^�̃X�P�[���̒���(plane��box�őS�R�Ⴄ�傫��������)
    /// </summary>
    /// <returns></returns>
    private float GetFloorScaleAdjust() { return m_isPlane ? 10 : 1; }


    //--------------------------------------------------------------------------------------
    /// �f�o�b�O
    //--------------------------------------------------------------------------------------

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
}
