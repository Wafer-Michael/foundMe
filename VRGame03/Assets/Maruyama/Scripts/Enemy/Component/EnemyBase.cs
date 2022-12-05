using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBase : MonoBehaviour
{
    EyeSearchRange m_eyeRange;

    [SerializeField]
    private List<GameObject> m_observeIsInEyeTargetObjects = new List<GameObject>();

    private ObserveIsInEyeTargets m_observeIsInEyeTargets;  //視界範囲に入ったらターゲット判定を取るターゲット
    public ObserveIsInEyeTargets GetObserveIsInEyeTargets() => m_observeIsInEyeTargets;

    private void Awake()
    {
        m_eyeRange = GetComponent<EyeSearchRange>();

        m_observeIsInEyeTargets = new ObserveIsInEyeTargets(m_observeIsInEyeTargetObjects, m_eyeRange);
    }

}
