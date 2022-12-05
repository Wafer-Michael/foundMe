using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using MaruUtility;

namespace StateNode
{


    public class BreadSeekTarget : NodeBase<EnemyBase>
    {
        #region メンバ変数

        private float m_nearRange = 3.0f;
        private float m_maxSpeed = 3.0f;
        private float m_turningPower = 1.0f;  //旋回する力
        private float m_lostSeekTime = 10.0f; //ロストしてから一定時間追従する時間
        private Vector3 m_targetPosition = new Vector3();

        private GameTimer m_timer = new GameTimer();

        //コンポ―ネント系----------------------------------

        //private ChaseTarget m_chaseTarget;
        //private WaitTimer m_waitTimer;
        private EnemyVelocityManager m_velocityManager;
        private TargetManager m_targetManager;
        private BreadCrumb m_bread;
        private RotationController m_rotationCtrl;
        //private StatusManagerBase m_statusManager;
        private EyeSearchRange m_eye;

        #endregion

        #region コンストラクタ
        public BreadSeekTarget(EnemyBase owner, float nearRange, float maxSpeed, float turningPower, float lostSeekTime)
            : base(owner)
        {
            m_nearRange = nearRange;
            m_maxSpeed = maxSpeed;
            m_turningPower = turningPower;
            m_lostSeekTime = lostSeekTime;

            //m_statusManager = owner.GetComponent<StatusManagerBase>();
            m_eye = owner.GetComponent<EyeSearchRange>();
            //m_chaseTarget = owner.GetComponent<ChaseTarget>();
            //m_waitTimer = owner.GetComponent<WaitTimer>();
            m_velocityManager = owner.GetComponent<EnemyVelocityManager>();
            m_targetManager = owner.GetComponent<TargetManager>();
        }
        #endregion

        #region Start,Update,Exit

        public override void OnStart()
        {
            var owner = GetOwner();

            //WaitTimerで一定時間見失ったら待機状態に移行することにする。
            //m_waitTimer.AddWaitTimer(GetType(), m_lostSeekTime, () => m_chaseTarget.TargetLost("BreadSeek"));
            m_timer.ResetTimer(m_lostSeekTime);  //ラムダで終了を通知する。

            var target = m_targetManager.GetCurrentTarget();

            m_bread = target?.GetComponent<BreadCrumb>();

            if (m_bread)
            {
                //初期ポジションのセット
                var position = CalcuTargetPosition(m_bread);
                if (position != null)
                {
                    m_targetPosition = (Vector3)position;
                }
                else
                {  //もしなかったら最新を取得
                    m_targetPosition = m_bread.GetNewPosition();
                }
            }

            m_rotationCtrl = owner.GetComponent<RotationController>();
        }

        public override bool OnUpdate()
        {
            if (!m_bread) {
                return true;
            }

            UpdateMove();

            m_timer.UpdateTimer();
            return m_timer.IsTimeUp;    //経過時間が終了したらtrue
        }

        public override void OnExit()
        {
            m_timer.AbsoluteEndTimer(false);
        }

        #endregion

        #region private関数

        private void UpdateMove()
        {
            var toVec = m_targetPosition - GetOwner().transform.position;
            var maxSpeed = m_maxSpeed;// * m_statusManager.GetBuffParametor().SpeedBuffMultiply;
            Vector3 force = CalcuVelocity.CalucSeekVec(m_velocityManager.velocity, toVec, maxSpeed);
            m_velocityManager.AddForce(force * m_turningPower);

            m_rotationCtrl.SetDirection(m_velocityManager.velocity);

            //目的地に到達したら
            if (Calculation.IsArrivalPosition(m_nearRange, GetOwner().transform.position, m_targetPosition, true))
            {
                NextRoute();
            }
        }

        private void NextRoute()
        {
            var newPosition = m_bread.GetNextPosition(m_targetPosition);

            if (newPosition != null)
            {
                m_targetPosition = (Vector3)newPosition;
            }
            else
            {
                m_targetPosition = m_bread.GetNewPosition();
            }
        }

        /// <summary>
        /// Rayの及ばない場所の取得
        /// </summary>
        /// <param name="bread">Breadデータ</param>
        /// <returns></returns>
        private Vector3? CalcuTargetPosition(BreadCrumb bread)
        {
            var positions = bread.GetCopyPositions();

            //最新のポジションから参照
            for (int i = positions.Count - 1; i > 0; i--)
            {
                //視界内のポジションを取得
                if (m_eye.IsInEyeRange(positions[i]))
                {
                    return positions[i];
                }
            }

            return null;
        }

        #endregion

        #region アクセッサ

        public void SetMaxSpeed(float maxSpeed)
        {
            m_maxSpeed = maxSpeed;
        }

        public void SetNearRange(float nearRange)
        {
            m_nearRange = nearRange;
        }

        public void SetLostSeekTime(float seekTime)
        {
            m_lostSeekTime = seekTime;
        }

        #endregion

    }

}
