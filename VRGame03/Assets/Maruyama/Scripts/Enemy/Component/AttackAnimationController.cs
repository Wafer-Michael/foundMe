using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackAnimationController : MonoBehaviour
{
    private GameTimer m_timer = new GameTimer();

    [SerializeField]
    private float m_time = float.MaxValue;

    [SerializeField]
    private float m_jumpForce = 5.0f;

    [SerializeField]
    private float m_moveSpeed = 5.0f;

    [SerializeField]
    private float m_gravitySpeed = 0.1f;

    private Vector3 m_jumpVector = Vector3.zero;

    public bool IsUpdate { get; set; } = false;

    private VelocityManager m_velocityManager;  //速度管理
    private Rigidbody m_rigidbody;              //リジッドボディ
    private TargetManager m_targetManager;      //ターゲット管理

    [SerializeField]
    private TriggerAction m_triggerAction;      //トリガーイベント登録

    public bool IsFloor { get; set; } = true;   //床にいるかどうかを設定

    private void Awake()
    {
        m_velocityManager = GetComponent<VelocityManager>();
        m_rigidbody = GetComponent<Rigidbody>();
        m_targetManager = GetComponent<TargetManager>();
    }

    private void Update()
    {
        if (!IsUpdate) {
            return;
        }

        m_timer.UpdateTimer();

        AnimationUpdate();

        if (IsEnd()) {
            EndProccess();
        }
    }

    private void AnimationUpdate()
    {
        //transform.position += m_jumpVector * Time.deltaTime;
        m_rigidbody.velocity = m_jumpVector;
        //m_velocityManager.velocity = m_jumpVector;
        m_jumpVector += new Vector3(0.0f, -m_gravitySpeed, 0.0f) * Time.deltaTime;
    }

    private void EndProccess() {
        Debug.Log("★★End");
        IsUpdate = false;
        var position = transform.position;
        transform.position = new Vector3(position.x, 0.15f, position.z);

        m_velocityManager.enabled = true;
        m_velocityManager.ResetAll();
        m_rigidbody.useGravity = true;
    }

    public void AttackStart() {
        if (IsUpdate) {
            return;
        }

        m_timer.ResetTimer(m_time);
        IsUpdate = true;

        m_velocityManager.ResetAll();
        m_velocityManager.enabled = false;
        m_rigidbody.useGravity = false;

        m_jumpVector = Vector3.zero;
        var toTarget = m_targetManager.GetCurrentTarget().transform.position - transform.position;
        m_jumpVector += toTarget.normalized * m_moveSpeed;
        m_jumpVector += new Vector3(0, m_jumpForce, 0);
        IsFloor = false;
    }

    private bool IsEnd() { return m_jumpVector.y <= 0.0f && IsFloor; }

    private void OnCollisionEnter(Collision other)
    {
        if (!IsUpdate) {
            return;
        }

        var damaged = other.gameObject.GetComponent<I_Damaged>();
        damaged?.Damaged(new DamageData(1.0f, this.gameObject, other.collider));
    }

    public void TestDebugLog() {
        Debug.Log("★★トリガー");
    }
}
