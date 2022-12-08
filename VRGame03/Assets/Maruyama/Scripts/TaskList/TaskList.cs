using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using System;
using System.Linq;

/// <summary>
/// ��̃^�X�N�̃x�[�X�N���X
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

    #region �����o�ϐ�

    private List<ChangeCompParam> m_changeParams = new List<ChangeCompParam>();

    protected OwnerType m_owner;
    protected OwnerType Owner => m_owner;
    protected OwnerType GetOwner() => m_owner;

    #endregion

    #region �R���X�g���N�^
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

    #region protected�֐�

    /// <summary>
    /// �؂�ւ���R���|�[�l���g�̏���
    /// </summary>
    protected virtual void ReserveChangeComponents() { }

    /// <summary>
    /// �J�n�ƏI�����ɐ؂�ւ���R���|�[�l���g�̒ǉ�
    /// </summary>
    /// <param name="behaviour">�؂�ւ���R���|�[�l���g�̃|�C���^</param>
    /// <param name="isStart">�X�^�[�g���ɂǂ����ɐ؂�ւ���</param>
    /// <param name="isExit">�I�����ɂǂ����ɐ؂�ւ��邩</param>
    protected void AddChangeComp(Behaviour behaviour, bool isStart, bool isExit)
    {
        if (behaviour == null)
        {  //nullptr�Ȃ�ǉ����Ȃ�
            return;
        }

        var param = new ChangeCompParam(behaviour, isStart, isExit);
        m_changeParams.Add(param);
    }

    #endregion

    #region private�֐�

    /// <summary>
    /// �o�^���ꂽ�R���|�[�l���g�̐؂�ւ����s��
    /// </summary>
    /// <param name="type">Start��Exit�̐ؑփ^�C�v</param>
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
/// �^�X�N�m�[�h�̊g��(Enter,Update,Exit)��o�^�\�ɂ����B
/// </summary>
/// <typeparam name="OwnerType"></typeparam>
public abstract class TaskNodeBase_Ex<OwnerType> : TaskNodeBase<OwnerType>
    where OwnerType : class
{
    #region �p�����[�^

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

    #region �R���X�g���N�^
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
    #region �^�X�N�N���X
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

    #region �����o�ϐ�
    /// <summary>
    /// ��`���ꂽ�^�X�N
    /// </summary>
    private Dictionary<EnumType, Task> m_defineTaskDictionary = new Dictionary<EnumType, Task>();

    /// <summary>
    /// ���ݐς܂�Ă���^�X�N
    /// </summary>
    private List<Task> m_currentTasks = new List<Task>();
    /// <summary>
    /// ���ݓ��삵�Ă���^�X�N
    /// </summary>
    private Task m_currentTask = null;
    /// <summary>
    /// ���ݓ��삵�Ă���^�X�N��Index
    /// </summary>
    private int m_currentIndex = 0;
    #endregion

    #region Update

    /// <summary>
    /// ���t���[���Ăԏ���(�O����Update�Ǘ�)
    /// </summary>
    public void UpdateTask()
    {
        if (IsEnd)
        {  //�I����ԂȂ珈�����s��Ȃ�
            return;
        }

        if (m_currentTask == null) //�J�����g��null�Ȃ�
        {
            m_currentTask = m_currentTasks[m_currentIndex];  //���݂̃^�X�N�̎擾
            m_currentTask.enter?.Invoke();
        }

        //�^�X�N��Update
        bool isEndOneTask = m_currentTask.update();

        //�^�X�N���I��������
        if (isEndOneTask)
        {
            EndOneTask();
        }
    }

    #endregion

    #region private�֐�

    /// <summary>
    /// ��̃^�X�N�̏I����
    /// </summary>
    private void EndOneTask()
    {
        m_currentTask?.exit?.Invoke();  //���݂̃^�X�N��Exit

        m_currentIndex++; //Index�̍X�V

        if (IsEnd)  //���̃^�X�N���Ȃ��Ȃ�
        {
            m_currentIndex = 0;
            m_currentTask = null;
            m_currentTasks.Clear();
            return;
        }

        m_currentTask = m_currentTasks[m_currentIndex]; //���̃^�X�N���擾
        m_currentTask.enter?.Invoke(); //���̃^�X�N��Enter
    }

    #endregion

    #region public�֐�

    /// <summary>
    /// �^�X�N�̒�`
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
    /// �^�X�N�̒�`
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
            Debug.Log("���ɒǉ�����Ă��܂��B");
            return;
        }
        m_defineTaskDictionary.Add(type, task);
    }

    /// <summary>
    /// �^�X�N�̓o�^
    /// </summary>
    /// <param name="type"></param>
    public void AddTask(EnumType type)
    {
        Task task = null;
        //���݂�����擾�ł���֐�
        var exist = m_defineTaskDictionary.TryGetValue(type, out task);
        if (exist == false)
        {
            Debug.Log("�^�X�N���o�^����Ă��܂���");
            return;
        }

        m_currentTasks.Add(task);
    }

    /// <summary>
    /// �����I��
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
    /// �����I�Ɏ��̃^�X�N�ɕύX����B
    /// </summary>
    public void ForceNextTask()
    {
        EndOneTask();
    }

    /// <summary>
    /// �I���������Ă΂Ȃ������I��
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

    #region �A�N�Z�b�T

    /// <summary>
    /// �S�Ẵ^�X�N���I�����Ă��邩�ǂ���
    /// </summary>
    public bool IsEnd =>
        m_currentTasks.Count <= m_currentIndex;

    /// <summary>
    /// �^�X�N�������Ă��邩�ǂ���
    /// </summary>
    public bool IsMoveTask => m_currentTask != null;

    /// <summary>
    /// ���ݐi�s���̃^�X�N�̃^�C�v���擾
    /// </summary>
    public EnumType CurrentTaskType => (m_currentTask == null) ? default(EnumType) : m_currentTask.type;

    /// <summary>
    /// �ǉ�����Ă���^�X�N�̃^�C�v���X�g
    /// </summary>
    public List<EnumType> CurrentTaskTypeList
    {
        get => m_currentTasks.Select(x => x.type).ToList();
    }

    /// <summary>
    /// ���݂̃C���f�b�N�X�̎擾
    /// </summary>
    public int CurrentIndex => m_currentIndex;

    #endregion
}
