using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponBase : MonoBehaviour
{
    [System.Serializable]
    public struct Parametor
    {
        public float speed;         //ショットスピード
        public float interval;      //ショット間隔
        public GameObject prefab;   //生成するプレハブ
        [SerializeField]
        private GameObject createPositionObject;    //弾生成する場所を記録するプレハブ
        public Vector3 CreatePosition => createPositionObject.transform.position;   //弾を生成する場所
    }

    [SerializeField]
    private Parametor m_param;  //パラメータ
    public Parametor Param => m_param;

    private GameTimer m_timer = new GameTimer();

    private void Start()
    {
        m_timer.ResetTimer(0);
    }

    private void Update()
    {
        m_timer.UpdateTimer();
    }

    /// <summary>
    /// 撃つ処理
    /// </summary>
    /// <returns>生成した弾オブジェクト</returns>
    public GameObject Shot()
    {
        return Shot(transform.forward);
    }

    /// <summary>
    /// 撃つ処理
    /// </summary>
    /// <param name="direction">撃つ方向</param>
    /// <returns>生成した弾オブジェクト</returns>
    public GameObject Shot(Vector3 direction)
    {
        if (!IsShot())  //撃てないなら撃たない。
        {
            return null;
        }

        //弾の生成
        var bulletObject = Instantiate(m_param.prefab, m_param.CreatePosition, Quaternion.identity);
        //弾に撃ったことを伝える。
        var bullet = bulletObject.GetComponent<BulletBase>();
        bullet?.Shot(direction, m_param.speed);

        m_timer.ResetTimer(m_param.interval);   //インターバルカウントの開始

        return bulletObject;
    }

    public bool IsShot()
    {
        return m_timer.IsTimeUp;
    }
}
