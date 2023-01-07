using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Oculus.Interaction;

public class UIScrollController : MonoBehaviour
{
    [SerializeField]
    private Vector3 m_maxScope = new Vector3(+0.0f, 0.0f, 0.0f); //������ő勗��

    [SerializeField]
    private Vector3 m_minScope = new Vector3(-0.0f, 0.0f, 0.0f); //������ŏ�����

    private Vector3 m_initializePosition;
    private GameObject m_initializeObject;
    private PointerEvent m_selectPoint;    //�I�����̃|�C���g

    private bool m_isTouch = false;

    private void Awake()
    {
        m_initializePosition = transform.position;
        m_initializeObject = Instantiate(new GameObject(), transform.position, transform.rotation);
    }

    public void Touch_Select(PointerEvent pointerEvent)
    {
        m_selectPoint = pointerEvent;
        m_isTouch = true;
    }

    public void Touch_Mover(PointerEvent pointerEvent)
    {
        if (!m_isTouch) {   //�^�b�`���łȂ��Ȃ�
            return;
        }

        transform.position = CalculatePosition(pointerEvent);
    }

    public void Touch_UnSelect(PointerEvent pointerEvent)
    {
        m_isTouch = false;
    }

    public Vector3 CalculatePosition(PointerEvent pointerEvent)
    {
        var result = pointerEvent.Pose.position;

        var toPoint = m_initializeObject.transform.InverseTransformPoint(pointerEvent.Pose.position);

        //x���ʃN�����v

        if (toPoint.x > m_maxScope.x) {
            toPoint.x = m_maxScope.x;
        }

        if (toPoint.x < m_minScope.x) {
            toPoint.x = m_minScope.x;
        }

        ////y���ʃN�����v

        if (toPoint.y > m_maxScope.y) {
            toPoint.y = m_maxScope.y;
        }

        if (toPoint.y < m_minScope.y) {
            toPoint.y = m_minScope.y;
        }

        return m_initializeObject.transform.position + (transform.rotation * toPoint);
    }

}
