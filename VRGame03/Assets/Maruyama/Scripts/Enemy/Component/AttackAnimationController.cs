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

    private VelocityManager m_velocityManager;
    private TargetManager m_targetManager;

    [SerializeField]
    private GameObject testTarget;

    private void Awake()
    {
        m_velocityManager = GetComponent<VelocityManager>();
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
        transform.position += m_jumpVector * Time.deltaTime;
        m_jumpVector += new Vector3(0.0f, -m_gravitySpeed, 0.0f) * Time.deltaTime;
    }

    private void EndProccess() {
        Debug.Log("ÅöÅöEnd");
        IsUpdate = false;
        var position = transform.position;
        transform.position = new Vector3(position.x, 0, position.z);

        m_velocityManager.enabled = true;
    }

    public void AttackStart() {
        if (IsUpdate) {
            return;
        }

        m_timer.ResetTimer(m_time);
        IsUpdate = true;

        m_velocityManager.ResetAll();
        m_velocityManager.enabled = false;

        m_jumpVector = Vector3.zero;
        var toTarget = m_targetManager.GetCurrentTarget().transform.position - transform.position;
        m_jumpVector += toTarget.normalized * m_moveSpeed * Time.deltaTime;
        m_jumpVector += new Vector3(0, m_jumpForce * Time.deltaTime, 0);
    }

    private bool IsEnd() { return transform.position.y < 0; }

    private void OnCollisionEnter(Collision other)
    {
        if (!IsUpdate)
        {
            return;
        }

        var damaged = other.gameObject.GetComponent<I_Damaged>();
        damaged?.Damaged(new DamageData(1.0f, this.gameObject, other.collider));
    }
}
