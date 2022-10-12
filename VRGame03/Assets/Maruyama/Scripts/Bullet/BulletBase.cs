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
    /// 撃つ処理
    /// </summary>
    /// <param name="direction">撃つ方向</param>
    /// <param name="weaponSpeed">武器ごとの発射スピード</param>
    public void Shot(Vector3 direction, float weaponSpeed = 1.0f)
    {
        m_velocityManager.velocity = direction * weaponSpeed * m_param.speed;
    }
}
