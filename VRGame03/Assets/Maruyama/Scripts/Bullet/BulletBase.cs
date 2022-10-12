using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(VelocityManager))]
public class BulletBase : MonoBehaviour
{
    [System.Serializable]
    public struct Parametor
    {
        public float speed;
    }

    [SerializeField]
    private Parametor m_param;
    public Parametor Param => m_param;

    private VelocityManager m_velocityManager;

    private void Awake()
    {
        m_velocityManager = GetComponent<VelocityManager>();
    }

    /// <summary>
    /// ������
    /// </summary>
    /// <param name="direction">������</param>
    /// <param name="weaponSpeed">���킲�Ƃ̔��˃X�s�[�h</param>
    public void Shot(Vector3 direction, float weaponSpeed = 1.0f)
    {
        m_velocityManager.velocity = direction.normalized * weaponSpeed * m_param.speed;
        transform.forward = direction;
    }
}
