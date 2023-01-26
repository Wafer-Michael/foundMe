using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//using MaruUtility;

[RequireComponent(typeof(Rigidbody))]
public class VelocityManager : MonoBehaviour
{
    #region メンバ変数

    private Rigidbody m_rigid;

    private Vector3 m_force = new Vector3();
    private Vector3 m_velocity = new Vector3();

    private bool m_isDeseleration = false;  //減速中かどうか
    public bool IsDeseleration => m_isDeseleration;
    private float m_deselerationPower = 1.0f;
    //private Vector3 m_deseleratironDirection = Vector3.zero;

    [SerializeField]
    private float m_maxSpeed = 2.0f;

    #endregion

    #region Awake,Update

    private void Awake()
    {
        m_rigid = GetComponent<Rigidbody>();
    }

    private void Update()
    {
        m_velocity.y += m_rigid.velocity.y - m_velocity.y;  //重力分加算する。
        //m_velocity += m_force * Time.deltaTime;
        m_velocity += m_force;                              //力の加算

        if (maru.UtilityMath.IsNaN(m_velocity)) {  //速度がNaNなら処理をしない。
            return;
        }
        
        m_rigid.velocity = m_velocity;

        //力のリセット
        ResetForce();

        //減速処理
        Deseleration();
    }

    #endregion

    #region private関数

    /// <summary>
    /// 減速処理
    /// </summary>
    private void Deseleration()
    {
        if (!m_isDeseleration) {
            return;
        }

        var force = maru.CalculateVelocity.SeekVec(velocity, -velocity, velocity.magnitude * m_deselerationPower);
        AddForce(force);
        //Debug.Log(velocity.magnitude);

        const float stopSpeed = 0.5f;
        if (velocity.magnitude <= stopSpeed) {
            m_isDeseleration = false;
            ResetAll();
        }
    }

    #endregion

    #region アクセッサ

    public Vector3 velocity
    {
        set { m_velocity = value; }
        get { return m_velocity; }
    }

    public void AddForce(Vector3 force)
    {
        //m_rigid.AddForce(force);
        m_force += force;
    }

    public Vector3 GetForce()
    {
        return m_force;
    }

    public void ResetVelocity()
    {
        m_velocity = Vector3.zero;
        m_rigid.velocity = Vector3.zero;
    }

    public void ResetForce()
    {
        m_force = Vector3.zero;
    }

    public void ResetAll()
    {
        ResetVelocity();
        ResetForce();
    }

    /// <summary>
    /// 減速開始
    /// </summary>
    /// <param name="power">減速する力</param>
    public void StartDeseleration(float power = 1.0f)
    {
        m_isDeseleration = true;
        //m_deseleratironDirection = -velocity;
        m_deselerationPower = power;
    }

    public void SetIsDeseleration(bool isDeseleration)
    {
        m_isDeseleration = isDeseleration;
    }

    /// <summary>
    /// 最大スピード
    /// </summary>
    /// <returns>最大スピード</returns>
    public float GetMaxSpeed() { return m_maxSpeed; }

    /// <summary>
    /// 減速の強さ
    /// </summary>
    public float deselerationPower
    {
        set { m_deselerationPower = value; }
        get { return m_deselerationPower; }
    }

    #endregion
}
