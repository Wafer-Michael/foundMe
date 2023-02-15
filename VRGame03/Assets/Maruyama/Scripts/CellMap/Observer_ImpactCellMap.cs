using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Observer_ImpactCellMap : MonoBehaviour
{
    [System.Serializable]
    public struct Parametor
    {
        [Header("脅威度変化時間")]
        public float comebackTime;   //脅威度変異時間
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
            if (!cell.IsActive()) {
                continue;
            }

            UpdateDangerValue(cell);
        }
    }

    private void UpdateDangerValue(ImpactCell cell)
    {
        const float COMEBACK_DANGERVALUE = 0.5f;                    //最終的に戻りたい数値
        float speed = COMEBACK_DANGERVALUE / m_param.comebackTime;  //経過スピード
        float dangerValue = cell.GetDangerValue();

        if (dangerValue == COMEBACK_DANGERVALUE) {
            return;
        }

        //戻りたい数値より、小さいなら
        if(dangerValue < COMEBACK_DANGERVALUE) {
            float value = dangerValue + (speed * Time.deltaTime);
            cell.SetDangerValue(Mathf.Clamp(value, 0, COMEBACK_DANGERVALUE));
        }

        //戻りたい数値より、大きいなら
        if(dangerValue > COMEBACK_DANGERVALUE) {
            float value = dangerValue - (speed * Time.deltaTime);
            cell.SetDangerValue(Mathf.Clamp(value, COMEBACK_DANGERVALUE, 1));
        }
    }
}
