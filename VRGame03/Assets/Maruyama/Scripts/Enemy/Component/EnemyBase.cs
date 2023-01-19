using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBase : MonoBehaviour
{
    EyeSearchRange m_eyeRange;

    private List<GameObject> m_observeIsInEyeTargetObjects = new List<GameObject>();

    private ObserveIsInEyeTargets m_observeIsInEyeTargets;  //視界範囲に入ったらターゲット判定を取るターゲット
    public ObserveIsInEyeTargets GetObserveIsInEyeTargets() => m_observeIsInEyeTargets;

    private void Awake()
    {
        m_eyeRange = GetComponent<EyeSearchRange>();

        //playerをターゲットに設定する仮処理
        if (m_observeIsInEyeTargetObjects.Count == 0)
        {
            var players = FindObjectsOfType<PlayerBase>();
            foreach(var player in players)
            {
                m_observeIsInEyeTargetObjects.Add(player.gameObject);
            }
        }

        m_observeIsInEyeTargets = new ObserveIsInEyeTargets(m_observeIsInEyeTargetObjects, m_eyeRange);
    }

}
