using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Observer_WayPointsMap : MonoBehaviour
{
    [System.Serializable]
    public struct Parametor
    {
        [Header("���Гx�ω�����")]
        public float comebackTime;   //���Гx�ψَ���
    }

    [SerializeField]
    private FieldWayPointsMap m_fieldWayPointMap;

    [SerializeField]
    private Parametor m_param = new Parametor() { comebackTime = 20.0f };

    private void Awake()
    {
        if (!m_fieldWayPointMap) {
            m_fieldWayPointMap = GetComponentInParent<FieldWayPointsMap>();
        }    
    }

    private void Update()
    {
        var nodes = AIDirector.Instance.GetWayPointsMap().GetGraph().GetNodes();
        foreach (var node in nodes) {
            UpdateDangerValue(node);
        }
    }

    private void UpdateDangerValue(AstarNode node)
    {
        const float COMEBACK_DANGERVALUE = 0.5f;                    //�ŏI�I�ɖ߂肽�����l
        float speed = COMEBACK_DANGERVALUE / m_param.comebackTime;  //�o�߃X�s�[�h
        float dangerValue = node.GetDangerValue();

        if (dangerValue == COMEBACK_DANGERVALUE)
        {
            //Debug.Log("������" + "�X�V�K�v�Ȃ�");
            return;
        }

        //�߂肽�����l���A�������Ȃ�
        if (dangerValue < COMEBACK_DANGERVALUE)
        {
            float value = dangerValue + (speed * Time.deltaTime);
            //Debug.Log("������" + value);
            node.SetDangerValue(Mathf.Clamp(value, 0, COMEBACK_DANGERVALUE));
        }

        //�߂肽�����l���A�傫���Ȃ�
        if (dangerValue > COMEBACK_DANGERVALUE)
        {
            float value = dangerValue - (speed * Time.deltaTime);
            //Debug.Log("������" + value);
            node.SetDangerValue(Mathf.Clamp(value, COMEBACK_DANGERVALUE, 1));
        }
    }
}
