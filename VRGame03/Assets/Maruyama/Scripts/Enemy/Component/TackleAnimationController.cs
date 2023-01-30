using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TackleAnimationController : MonoBehaviour
{
    private enum TaskEnum
    {
        Preliminary,    //�\������
        Attack,         //�U��
        Wait,           //�ҋ@
    }

    private TaskList<TaskEnum> m_taskList = new TaskList<TaskEnum>();

    private void Awake()
    {
        DefineTask();
    }

    private void Update()
    {
        //�^�X�N���I�����Ă�����
        if (m_taskList.IsEnd) {
            return;
        }

        m_taskList.UpdateTask();    //�^�X�N�̃A�b�v�f�[�g
    }

    private void DefineTask()
    {
        //�\������

        //�U���^�X�N

        //�ҋ@
    }

    private void SelectTask()
    {
        TaskEnum[] tasks = {
            TaskEnum.Preliminary,
            TaskEnum.Attack,
            TaskEnum.Wait
        };

        foreach(var task in tasks) {
            m_taskList.AddTask(task);
        }
    }
}
