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

    //private RotationController m_rotationController;

    private void Awake()
    {
        //回転コントローラーがないなら
        if(m_param.rotationControllerAxis == null) {
            m_param.rotationControllerAxis = GetComponent<RotationController>();
        }
        m_param.DefaultDirection = Param.rotationControllerAxis.gameObject.transform.forward;   //初期方向を取得

        CreateNode();
    }
    private void Update()
    {
        //if (PlayerInputer.IsDebugKeyDown(KeyCode.Y))
        //{
        //    Open(FindObjectOfType<PCPlayer>().gameObject);
        //}

        //var requesterToOwner = transform.position - FindObjectOfType<PCPlayer>().gameObject.transform.position;
        //float newDot = Vector3.Dot(requesterToOwner, transform.forward);
        //Debug.Log(newDot);

        m_stateMachine.OnUpdate();
    }

    private void CreateNode()
    {
        m_stateMachine.AddNode(State.Idle, null);

        m_stateMachine.AddNode(State.Open, new StateNode.Door_Open(this));  //ドアを開く
    }

    public void Access(GameObject other)
    {
        Open(other);
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


}
