using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(RotationController))]
public class OpenDoor : MonoBehaviour, I_InputAccess
{
    [System.Serializable]
    public struct Parametor {
        public float degree;   //回転させたい角度
        public float speed;    //回転させる速度
        public RotationController rotationControllerAxis;   //回転させたい軸
        public float openIdleTime;

        public float Radian => degree * Mathf.Deg2Rad; 
        public Vector3 DefaultDirection { get; set; }        //初期方向
        public GameObject openRequester { get; set; }        //ドアを開ける人
    }

    enum State {
        Idle,
        Open,
        OpenIdle,
        Close,
    }

    struct TransitionMember {
        
    }

    [SerializeField]
    private Parametor m_param;
    public Parametor Param => m_param;

    private Vector3 m_openDirection;    //開く方向
    public Vector3 OpenDirection
    {
        private set => m_openDirection = value;
        get => m_openDirection;
    }

    private StateMachine<OpenDoor, State, TransitionMember> m_stateMachine = new StateMachine<OpenDoor, State, TransitionMember>();

    private DoorLock m_doorLock = null;

    [SerializeField]
    AudioSource m_openSE; // 開閉時のSE

    private void Awake()
    {
        m_doorLock = GetComponent<DoorLock>();

        //回転コントローラーがないなら
        if(m_param.rotationControllerAxis == null) {
            m_param.rotationControllerAxis = GetComponent<RotationController>();
        }
        m_param.DefaultDirection = Param.rotationControllerAxis.gameObject.transform.forward;   //初期方向を取得

        CreateNode();

        CreateEdge();
    }
    private void Update()
    {
        m_stateMachine.OnUpdate();
    }

    
    private void CreateNode()
    {
        m_stateMachine.AddNode(State.Idle, null);

        m_stateMachine.AddNode(State.Open, new StateNode.Door_Open(this));              //ドアを開く

        m_stateMachine.AddNode(State.OpenIdle, new StateNode.Door_OpenIdle(this));      //開いた状態

        m_stateMachine.AddNode(State.Close, new StateNode.Door_Close(this));            //閉じた状態
    }

    private void CreateEdge()
    {
        m_stateMachine.AddEdge(State.Open, State.OpenIdle, IsTrue, (int)State.OpenIdle, true);

        m_stateMachine.AddEdge(State.OpenIdle, State.Close, IsTrue, (int)State.Close, true);

        m_stateMachine.AddEdge(State.Close, State.Idle, IsTrue, (int)State.Idle, true);
    }

    private bool IsTrue(ref TransitionMember member)
    {
        return true;
    }

    public void Access(GameObject other)
    {
        //ロック状態なら、
        if(m_doorLock.IsLock){
            m_doorLock.AccessKey(other);
        }
        else {
            m_openSE.PlayOneShot(m_openSE.clip);
            Open(other);
        }
    }

    public GameObject GetGameObject() => gameObject;

    public void Open(GameObject other)
    {
        if(m_stateMachine.GetNowType() == State.Open) {
            return;
        }

        m_param.openRequester = other;
        ChangeState(State.Open);
    }

    private void ChangeState(State state)
    {
        m_stateMachine.ChangeState(state, (int)state);
    }

}

namespace StateNode
{
    class Door_Open : EnemyStateNodeBase<OpenDoor>
    {
        public Door_Open(OpenDoor owner) :
            base(owner)
        {}

        public override void OnStart()
        {
            base.OnStart();

            var rotationContorller = GetOwner().Param.rotationControllerAxis;
            rotationContorller.SetSpeed(GetOwner().Param.speed);
            rotationContorller.SetDirection(CalculateRotationDirection());
        }

        public override bool OnUpdate()
        {
            return IsEnd();
        }

        private Vector3 CalculateRotationDirection()
        {
            var result = Vector3.zero;

            var param = GetOwner().Param;
            var defaultDirection = param.DefaultDirection;
            var degree = param.degree * ComvertRequester();
            result = Quaternion.AngleAxis(degree, Vector3.up) * defaultDirection;

            return result;
        }

        private int ComvertRequester()
        {
            var param = GetOwner().Param;
            var requesterToOwner = GetOwner().transform.position - param.openRequester.transform.position;
            float newDot = Vector3.Dot(requesterToOwner, GetOwner().transform.right);

            return newDot > 0 ? -1 : 1;
        }

        private bool IsEnd()
        {
            return !GetOwner().Param.rotationControllerAxis.IsRotation;
        }
    }


    class Door_OpenIdle : EnemyStateNodeBase<OpenDoor>
    {
        private GameTimer m_timer = new GameTimer();

        public Door_OpenIdle(OpenDoor owner) :
             base(owner)
        { }

        public override void OnStart()
        {
            base.OnStart();

            m_timer.ResetTimer(GetOwner().Param.openIdleTime);
        }

        public override bool OnUpdate()
        {
            m_timer.UpdateTimer();

            return m_timer.IsTimeUp;
        }
    }

    class Door_Close : EnemyStateNodeBase<OpenDoor>
    {
        private GameTimer m_timer = new GameTimer();

        public Door_Close(OpenDoor owner) :
             base(owner)
        { }

        public override void OnStart()
        {
            var param = GetOwner().Param;
            param.rotationControllerAxis.SetDirection(param.DefaultDirection);
        }

        public override bool OnUpdate()
        {
            return !GetOwner().Param.rotationControllerAxis.IsRotation;
        }
    }


}
