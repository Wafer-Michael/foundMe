using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Oculus.Interaction;

public class UIScrollController : MonoBehaviour
{
    [SerializeField]
    private Vector2 m_maxScope = new Vector2(+0.1f, +0.1f); //������ő勗��

    [SerializeField]
    private Vector2 m_minScope = new Vector2(-0.1f, -0.1f); //������ŏ�����

    private Vector3 m_initializePosition;
    private GameObject m_initializeObject;

    private bool m_isTouch = false;

    private void Awake()
    {
        m_initializePosition = transform.position;
        m_initializeObject = new GameObject("TouchUIInitialize");
        m_initializeObject.transform.position = transform.position;
        m_initializeObject.transform.rotation = transform.rotation;
    }

    /// <summary>
    /// UnityEentWrap�ɓo�^����
    /// </summary>
    /// <param name="pointerEvent"></param>
    public void Touch_Select(PointerEvent pointerEvent)
    {
        m_isTouch = true;
    }

    /// <summary>
    /// UnityEvetWrap�ɓo�^����
    /// </summary>
    /// <param name="pointerEvent"></param>
    public void Touch_Mover(PointerEvent pointerEvent)
    {
        if (!m_isTouch) {   //�^�b�`���łȂ��Ȃ�
            return;
        }

        transform.position = CalculatePosition(pointerEvent);
    }

    /// <summary>
    /// UnityEventWrap�ɓo�^����
    /// </summary>
    /// <param name="pointerEvent"></param>
    public void Touch_UnSelect(PointerEvent pointerEvent)
    {
        m_isTouch = false;
    }

    /// <summary>
    /// �ʒu�̌v�Z
    /// </summary>
    /// <param name="pointerEvent">�|�C���^�[�C�x���g</param>
    /// <returns>�v�Z�����ʒu</returns>
    public Vector3 CalculatePosition(PointerEvent pointerEvent)
    {
        var toPoint = maru.Utility.InverseTransformPoint(m_initializePosition, transform.rotation, pointerEvent.Pose.position);

        //���ꂼ��̃N�����v
        toPoint.x = Mathf.Clamp(toPoint.x, m_minScope.x, m_maxScope.x);
        toPoint.y = Mathf.Clamp(toPoint.y, m_minScope.y, m_maxScope.y);

        return m_initializePosition + (transform.rotation * toPoint);
    }

}
