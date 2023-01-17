using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Observer_ImpactCellMap : MonoBehaviour
{
    [System.Serializable]
    public struct Parametor
    {
        [Header("���Гx�ω�����")]
        public float comebackTime;   //���Гx�ψَ���
    }

    [SerializeField]
    private Parametor m_param = new Parametor() {
        comebackTime = 20.0f
    };

    [SerializeField]
    private FieldImpactCellMap m_fieldCellMap;

    private void Update()
    {
        foreach(var cell in m_fieldCellMap.GetCellMap().GetCells())
        {
            UpdateDangerValue(cell);
        }
    }

    private void UpdateDangerValue(ImpactCell cell)
    {
        const float COMEBACK_DANGERVALUE = 0.5f;                    //�ŏI�I�ɖ߂肽�����l
        float speed = COMEBACK_DANGERVALUE / m_param.comebackTime;  //�o�߃X�s�[�h
        float dangerValue = cell.GetDangerValue();

        if (dangerValue == COMEBACK_DANGERVALUE) {
            return;
        }

        //�߂肽�����l���A�������Ȃ�
        if(dangerValue < COMEBACK_DANGERVALUE)
        {
            float value = dangerValue + (speed * Time.deltaTime);
            cell.SetDangerValue(Mathf.Clamp(value, 0, COMEBACK_DANGERVALUE));
        }

        //�߂肽�����l���A�傫���Ȃ�
        if(dangerValue > COMEBACK_DANGERVALUE)
        {
            float value = dangerValue - (speed * Time.deltaTime);
            cell.SetDangerValue(Mathf.Clamp(value, COMEBACK_DANGERVALUE, 1));
        }
    }
}
