using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// �^�[�Q�b�g�����������Ƃ��ɐ�������f�[�^
/// </summary>
public struct TargetLostData
{
    public GameObject target;       //������������
    public Vector3 lostPosition;    //���������ꏊ

    public TargetLostData(GameObject target, Vector3 lostPosition)
    {
        this.target = target;
        this.lostPosition = lostPosition;
    }
}

/// <summary>
/// �^�[�Q�b�g�̃f�[�^
/// </summary>
public struct TargetData
{
    public GameObject target;       //�^�[�Q�b�g�̃Q�[���I�u�W�F�N�g
    public float priority;          //�D��x
    public TargetLostData lostData; //���������Ƃ��̃f�[�^
    public Targeted targeted;

    public TargetData(GameObject target)
    {
        this.target = target;
        this.priority = 0;
        this.lostData = new TargetLostData();
        targeted = target ? target.GetComponent<Targeted>() : null;
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

    /// <summary>
    /// �^�[�Q�b�g�����������ꏊ���L�^����B
    /// </summary>
    /// <returns>�^�[�Q�b�g�����������ꏊ</returns>
    public Vector3 GetLostTargetPosition() { return m_currentData.lostData.lostPosition; }

    /// <summary>
    /// ���������ꏊ�ւ̕����x�N�g�����擾
    /// </summary>
    /// <returns>���������ꏊ�ւ̕����x�N�g��</returns>
    public Vector3 CalculateSelfLostPositionVector() { return GetLostTargetPosition() - transform.position; }

    /// <summary>
    /// �Ώۂ����E����O�ꂽ���Ƃ�`����B
    /// </summary>
    public void CallLostTarget()
    {
        //�^�[�Q�b�g�������Ă��Ȃ������珈�����s�\
        if (!HasTarget()) {
            return;
        }

        var target = GetCurrentTarget();
        m_currentData.lostData = new TargetLostData(target, target.transform.position);
    }
}
