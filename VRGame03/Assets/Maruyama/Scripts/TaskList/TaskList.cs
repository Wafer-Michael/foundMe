using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using System;
using System.Linq;

/// <summary>
/// 一つのタスクのベースクラス
/// </summary>
public abstract class TaskNodeBase
{
    public abstract void OnEnter();
    public abstract bool OnUpdate();
    public abstract void OnExit();
}

public abstract class TaskNodeBase<OwnerType> : TaskNodeBase
    where OwnerType : class
{
    private enum EnableChangeType
    {
        Start,
        Exit,
    }

    #region メンバ変数

    private List<ChangeCompParam> m_changeParams = new List<ChangeCompParam>();

    protected OwnerType m_owner;
    protected OwnerType Owner => m_owner;
    protected OwnerType GetOwner() => m_owner;

    #endregion

    #region コンストラクタ
    public TaskNodeBase(OwnerType owner)
    {
        m_owner = owner;
    }
    #endregion

    #region Enter,Exit

    public override void OnEnter()
    {
        ReserveChangeComponents();

        ChangeComps(EnableChangeType.Start);
    }

    public override void OnExit()
    {
        ChangeComps(EnableChangeType.Exit);
    }

    #endregion

    #region protected関数

    /// <summary>
    /// 切り替えるコンポーネントの準備
    /// </summary>
    protected virtual void ReserveChangeComponents() { }

    /// <summary>
    /// 開始と終了時に切り替えるコンポーネントの追加
    /// </summary>
    /// <param name="behaviour">切り替えるコンポーネントのポインタ</param>
    /// <param name="isStart">スタート時にどっちに切り替える</param>
    /// <param name="isExit">終了時にどっちに切り替えるか</param>
    protected void AddChangeComp(Behaviour behaviour, bool isStart, bool isExit)
    {
        if (behaviour == null)
        {  //nullptrなら追加しない
            return;
        }

        var param = new ChangeCompParam(behaviour, isStart, isExit);
        m_changeParams.Add(param);
    }

    #endregion

    #region private関数

    /// <summary>
    /// 登録されたコンポーネントの切り替えを行う
    /// </summary>
    /// <param name="type">StartかExitの切替タイプ</param>
    private void ChangeComps(EnableChangeType type)
    {
        foreach (var param in m_changeParams)
        {
            bool isEnable = type switch
            {
                EnableChangeType.Start => param.isStart,
                EnableChangeType.Exit => param.isExit,
                _ => false
            };

            param.behaviour.enabled = isEnable;
        }
    }

    #endregion
}

/// <summary>
/// タスクノードの拡張(Enter,Update,Exit)を登録可能にした。
/// </summary>
/// <typeparam name="OwnerType"></typeparam>
public abstract class TaskNodeBase_Ex<OwnerType> : TaskNodeBase<OwnerType>
    where OwnerType : class
{
    #region パラメータ

    public struct ActionParametor
    {
        public Action enter;
        public Action update;
        public Action exit;

        public ActionParametor(Action enter, Action update, Action exit)
        {
            this.enter = enter;
            this.update = update;
            this.exit = exit;
        }
    }

    #endregion

    private ActionParametor m_actionParam = new ActionParametor();

    #region コンストラクタ
    public TaskNodeBase_Ex(OwnerType owner, ActionParametor param = new ActionParametor())
        : base(owner)
    {
        m_actionParam = param;
    }
    #endregion

    #region Enter,Update,Exit

    public override void OnEnter()
    {
        base.OnEnter();

        m_actionParam.enter?.Invoke();
    }

    public override bool OnUpdate()
    {
        m_actionParam.update?.Invoke();
        return true;
    }

    public override void OnExit()
    {
        base.OnExit();

        m_actionParam.exit?.Invoke();
    }

    #endregion
}

public class TaskList<EnumType>
{
    #region タスククラス
    private class Task
    {
        public EnumType type;
        public Action enter;
        public Func<bool> update;
        public Action exit;

        public Task(EnumType type, Action enter, Func<bool> update, Action exit)
        {
            this.type = type;
            this.enter = enter;
            this.update = update ?? delegate { return true; };
            this.exit = exit;
        }

        public Task(EnumType type, TaskNodeBase task)
        {
            this.type = type;
            this.enter = task.OnEnter;
            this.update = task.OnUpdate;
            this.exit = task.OnExit;
        }
    }
    #endregion

    #region メンバ変数
    /// <summary>
    /// 定義されたタスク
    /// </summary>
    private Dictionary<EnumType, Task> m_defineTaskDictionary = new Dictionary<EnumType, Task>();

    /// <summary>
    /// 現在積まれているタスク
    /// </summary>
    private List<Task> m_currentTasks = new List<Task>();
    /// <summary>
    /// 現在動作しているタスク
    /// </summary>
    private Task m_currentTask = null;
    /// <summary>
    /// 現在動作しているタスクのIndex
    /// </summary>
    private int m_currentIndex = 0;
    #endregion

    #region Update

    /// <summary>
    /// 毎フレーム呼ぶ処理(外部でUpdate管理)
    /// </summary>
    public void UpdateTask()
    {
        if (IsEnd)
        {  //終了状態なら処理を行わない
            return;
        }

        if (m_currentTask == null) //カレントがnullなら
        {
            m_currentTask = m_currentTasks[m_currentIndex];  //現在のタスクの取得
            m_currentTask.enter?.Invoke();
        }

        //タスクのUpdate
        bool isEndOneTask = m_currentTask.update();

        //タスクが終了したら
        if (isEndOneTask)
        {
            EndOneTask();
        }
    }

    #endregion

    #region private関数

    /// <summary>
    /// 一つのタスクの終了時
    /// </summary>
    private void EndOneTask()
    {
        m_currentTask?.exit?.Invoke();  //現在のタスクのExit

        m_currentIndex++; //Indexの更新

        if (IsEnd)  //次のタスクがないなら
        {
            m_currentIndex = 0;
            m_currentTask = null;
            m_currentTasks.Clear();
            return;
        }

        m_currentTask = m_currentTasks[m_currentIndex]; //次のタスクを取得
        m_currentTask.enter?.Invoke(); //次のタスクのEnter
    }

    #endregion

    #region public関数

    /// <summary>
    /// タスクの定義
    /// </summary>
    /// <param name="type">EnumType</param>
    /// <param name="enter"></param>
    /// <param name="update"></param>
    /// <param name="exit"></param>
    public void DefineTask(EnumType type, TaskNodeBase task)
    {
        DefineTask(type, task.OnEnter, task.OnUpdate, task.OnExit);
    }

    /// <summary>
    /// タスクの定義
    /// </summary>
    /// <param name="type">EnumType</param>
    /// <param name="enter"></param>
    /// <param name="update"></param>
    /// <param name="exit"></param>
    public void DefineTask(EnumType type, Action enter, Func<bool> update, Action exit)
    {
        var task = new Task(type, enter, update, exit);
        var exist = m_defineTaskDictionary.ContainsKey(type);
        if (exist)
        {
            Debug.Log("既に追加されています。");
            return;
        }
        m_defineTaskDictionary.Add(type, task);
    }

    /// <summary>
    /// タスクの登録
    /// </summary>
    /// <param name="type"></param>
    public void AddTask(EnumType type)
    {
        Task task = null;
        //存在したら取得できる関数
        var exist = m_defineTaskDictionary.TryGetValue(type, out task);
        if (exist == false)
        {
            Debug.Log("タスクが登録されていません");
            return;
        }

        m_currentTasks.Add(task);
    }

    /// <summary>
    /// 強制終了
    /// </summary>
    public void ForceStop()
    {
        if (m_currentTask != null)
        {
            m_currentTask.exit?.Invoke();
        }
        m_currentTask = null;
        m_currentTasks.Clear();
        m_currentIndex = 0;
    }

    /// <summary>
    /// 強制的に次のタスクに変更する。
    /// </summary>
    public void ForceNextTask()
    {
        EndOneTask();
    }

    /// <summary>
    /// 終了処理を呼ばない強制終了
    /// </summary>
    public void ForceReset(bool isExitFunc = false)
    {
        if (isExitFunc)
        {
            m_currentTask.exit?.Invoke();
        }

        m_currentTask = null;
        m_currentTasks.Clear();
        m_currentIndex = 0;
    }

    #endregion

    #region アクセッサ

    /// <summary>
    /// 全てのタスクが終了しているかどうか
    /// </summary>
    public bool IsEnd =>
        m_currentTasks.Count <= m_currentIndex;

    /// <summary>
    /// タスクが動いているかどうか
    /// </summary>
    public bool IsMoveTask => m_currentTask != null;

    /// <summary>
    /// 現在進行中のタスクのタイプを取得
    /// </summary>
    public EnumType CurrentTaskType => (m_currentTask == null) ? default(EnumType) : m_currentTask.type;

    /// <summary>
    /// 追加されているタスクのタイプリスト
    /// </summary>
    public List<EnumType> CurrentTaskTypeList
    {
        get => m_currentTasks.Select(x => x.type).ToList();
    }

    /// <summary>
    /// 現在のインデックスの取得
    /// </summary>
    public int CurrentIndex => m_currentIndex;

    #endregion
}
