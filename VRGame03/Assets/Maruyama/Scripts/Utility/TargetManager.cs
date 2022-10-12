using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// �^�[�Q�b�g�����������Ƃ��ɐ�������f�[�^
/// </summary>
public struct TargetLostData
{
    
}

/// <summary>
/// �^�[�Q�b�g�̃f�[�^
/// </summary>
public struct TargetData
{
    public GameObject target;       //�^�[�Q�b�g�̃Q�[���I�u�W�F�N�g
    public float priority;          //�D��x
    public TargetLostData lostData; //���������Ƃ��̃f�[�^

    public TargetData(GameObject target)
    {
        this.target = target;
        this.priority = 0;
        this.lostData = new TargetLostData();
    }
}

/// <summary>
/// �^�[�Q�b�g�Ǘ��̃R���|�[�l���g
/// </summary>
public class TargetManager : MonoBehaviour
{
    public TargetData m_currentData;    //���݂̃^�[�Q�b�g

    /// <summary>
    /// �^�[�Q�b�g�������Ă��邩�ǂ���
    /// </summary>
    /// <returns>�����Ă���Ȃ�true</returns>
    public bool HasTarget()
    {
        return m_currentData.target != null ? true : false;
    }

    /// <summary>
    /// ���݂̃^�[�Q�b�g��ݒ肷��B
    /// </summary>
    /// <param name="target">�^�[�Q�b�g</param>
    public void SetCurrentTarget(GameObject target)
    {
        m_currentData = new TargetData(target);
    }

    public GameObject GetCurrentTarget()
    {
        return m_currentData.target;
    }

    public Vector3 CalculateSelfToTargetVector()
    {
        return GetCurrentTarget().transform.position - transform.position;
    }
}
