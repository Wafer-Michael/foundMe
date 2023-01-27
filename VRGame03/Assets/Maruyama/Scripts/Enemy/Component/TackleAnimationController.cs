using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TackleAnimationController : MonoBehaviour
{
    private enum TaskEnum
    {
        Preliminary,    //予備動作
        Attack,         //攻撃
        Wait,           //待機
    }

    private TaskList<TaskEnum> m_taskList = new TaskList<TaskEnum>();

    private void Awake()
    {
        DefineTask();
    }

    private void Update()
    {
        //タスクが終了していたら
        if (m_taskList.IsEnd) {
            return;
        }

        m_taskList.UpdateTask();    //タスクのアップデート
    }

    private void DefineTask()
    {
        //予備動作

        //攻撃タスク

        //待機
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
