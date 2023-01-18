using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Observer_WayPointsMap : MonoBehaviour
{
    [System.Serializable]
    public struct Parametor
    {
        [Header("脅威度変化時間")]
        public float comebackTime;   //脅威度変異時間
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
        const float COMEBACK_DANGERVALUE = 0.5f;                    //最終的に戻りたい数値
        float speed = COMEBACK_DANGERVALUE / m_param.comebackTime;  //経過スピード
        float dangerValue = node.GetDangerValue();

        if (dangerValue == COMEBACK_DANGERVALUE)
        {
            //Debug.Log("★★★" + "更新必要なし");
            return;
        }

        //戻りたい数値より、小さいなら
        if (dangerValue < COMEBACK_DANGERVALUE)
        {
            float value = dangerValue + (speed * Time.deltaTime);
            //Debug.Log("★★★" + value);
            node.SetDangerValue(Mathf.Clamp(value, 0, COMEBACK_DANGERVALUE));
        }

        //戻りたい数値より、大きいなら
        if (dangerValue > COMEBACK_DANGERVALUE)
        {
            float value = dangerValue - (speed * Time.deltaTime);
            //Debug.Log("★★★" + value);
            node.SetDangerValue(Mathf.Clamp(value, COMEBACK_DANGERVALUE, 1));
        }
    }
}
