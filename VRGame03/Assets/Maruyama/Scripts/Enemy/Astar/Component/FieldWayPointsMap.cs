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

    private DebugGraphDraw m_debugGraphDraw;    //�O���t�̃f�o�b�O�\���p
    [SerializeField]
    private GameObject m_debugNodePrefab;       //�f�o�b�O�p�̃m�[�hPrefab

    private void Awake()
    {
        m_wayPointsMap = new WayPointsMap();

        //�����m�[�h����
        m_factoryParametor.rect = CalculateFieldRect();
        m_wayPointsMap.CreateWayPointsMap(m_factoryParametor);

        //�O���t�̃f�o�b�O�\��
        m_debugGraphDraw = new DebugGraphDraw(this, m_wayPointsMap.GetGraph());
        m_debugGraphDraw.CreateDebugNodes();
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
}
