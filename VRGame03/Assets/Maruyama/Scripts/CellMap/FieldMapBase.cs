using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FieldMapBase : MonoBehaviour
{
    [SerializeField]
    private GameObject m_floorObject;   //�}�b�v��W�J���鏰�I�u�W�F�N�g
    protected GameObject GetFloorObject() => m_floorObject;
    protected bool HasFloorObject() => m_floorObject != null;

    [SerializeField]
    private bool m_isPlane = true;
    protected bool IsPlane => m_isPlane;

    /// <summary>
    /// �t�B�[���h�p�̎l�p�͈̓f�[�^���v�Z
    /// </summary>
    /// <returns></returns>
    protected maru.Rect CalculateFloorRect()
    {
        var rect = new maru.Rect();

        //���I�u�W�F�N�g�̐ݒ肪���Ă���Ȃ�A���ɍ��킹��rect�𐶐�
        if (m_floorObject)
        {
            rect.centerPosition = m_floorObject.transform.position;
            rect.width = m_floorObject.transform.localScale.x * GetFloorScaleAdjust();
            rect.depth = m_floorObject.transform.localScale.z * GetFloorScaleAdjust();
        }

        return rect;
    }

    /// <summary>
    /// ���f�[�^�̃X�P�[���̒���(plane��box�őS�R�Ⴄ�傫��������)
    /// </summary>
    /// <returns></returns>
    private float GetFloorScaleAdjust() { return m_isPlane ? 10 : 1; }
}
