using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(TargetManager))]
[RequireComponent(typeof(EyeSearchRange))]
[RequireComponent(typeof(RotationController))]
public class Tester_ShotEnemy : EnemyBase
{
    [SerializeField]
    List<WeaponBase> m_weapons = new List<WeaponBase>();

    EyeSearchRange m_eyeRange;
    TargetManager m_targetManager;
    RotationController m_rotationController;

    [SerializeField]
    List<GameObject> m_searchTargets = new List<GameObject>();

    private void Awake()
    {
        m_eyeRange = GetComponent<EyeSearchRange>();
        m_targetManager = GetComponent<TargetManager>();
        m_rotationController = GetComponent<RotationController>();
    }

    private void Update()
    {
        UpdateTargetManager();
        UpdateRotation();
        Shot();
    }

    void UpdateTargetManager()
    {
        foreach(var target in m_searchTargets)
        {
            if (m_eyeRange.IsInEyeRange(target))
            {
                m_targetManager.SetCurrentTarget(target);
            }
            else
            {
                m_targetManager.SetCurrentTarget(null);
            }
        }
    }

    private void UpdateRotation()
    {
        if (!m_targetManager.HasTarget())
        {
            return;
        }

        m_rotationController.SetDirect(m_targetManager.CalculateSelfToTargetVector());
    }

    void Shot()
    {
        if (!m_targetManager.HasTarget())
        {
            return;
        }

        foreach (var weapon in m_weapons)
        {
            weapon?.Shot(m_targetManager.CalculateSelfToTargetVector());
        }
    }
}
