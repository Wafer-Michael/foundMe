using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using MaruUtility;

namespace StateNode
{


    public class BreadSeekTarget : NodeBase<EnemyBase>
    {
        #region �����o�ϐ�

        private float m_nearRange = 3.0f;
        private float m_maxSpeed = 3.0f;
        private float m_turningPower = 1.0f;  //���񂷂��
        private float m_lostSeekTime = 10.0f; //���X�g���Ă����莞�ԒǏ]���鎞��
        private Vector3 m_targetPosition = new Vector3();

        private GameTimer m_timer = new GameTimer();

        //�R���|�\�l���g�n----------------------------------

        //private ChaseTarget m_chaseTarget;
        //private WaitTimer m_waitTimer;
        private VelocityManager m_velocityManager;
        private TargetManager m_targetManager;
        private BreadCrumb m_bread;
        private RotationController m_rotationCtrl;
        //private StatusManagerBase m_statusManager;
        private EyeSearchRange m_eye;

        #endregion

        #region �R���X�g���N�^
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
            m_velocityManager = owner.GetComponent<VelocityManager>();
            m_targetManager = owner.GetComponent<TargetManager>();
        }
        #endregion

        #region Start,Update,Exit

        public override void OnStart()
        {
            var owner = GetOwner();

            //WaitTimer�ň�莞�Ԍ���������ҋ@��ԂɈڍs���邱�Ƃɂ���B
            //m_waitTimer.AddWaitTimer(GetType(), m_lostSeekTime, () => m_chaseTarget.TargetLost("BreadSeek"));
            m_timer.ResetTimer(m_lostSeekTime);  //�����_�ŏI����ʒm����B

            var target = m_targetManager.GetCurrentTarget();

            m_bread = target?.GetComponent<BreadCrumb>();

            if (m_bread)
            {
                //�����|�W�V�����̃Z�b�g
                var position = CalcuTargetPosition(m_bread);
                if (position != null)
                {
                    m_targetPosition = (Vector3)position;
                }
                else
                {  //�����Ȃ�������ŐV���擾
                    m_targetPosition = m_bread.GetNewPosition();
                }
            }

            m_rotationCtrl = owner.GetComponent<RotationController>();

            Debug.Log("��Start_BreadCrumbSeek");
        }

        public override bool OnUpdate()
        {
            if (!m_bread) {
                return true;
            }

            UpdateMove();

            Debug.Log("BreadCrumbSeek");

            m_timer.UpdateTimer();
            return m_timer.IsTimeUp;    //�o�ߎ��Ԃ��I��������true
        }

        public override void OnExit()
        {
            m_timer.AbsoluteEndTimer(false);
        }

        #endregion

        #region private�֐�

        private void UpdateMove()
        {
            var toVec = m_targetPosition - GetOwner().transform.position;
            var maxSpeed = m_maxSpeed;// * m_statusManager.GetBuffParametor().SpeedBuffMultiply;
            Vector3 force = maru.CalculateVelocity.CalucSeekVec(m_velocityManager.velocity, toVec, maxSpeed);
            m_velocityManager.AddForce(force * m_turningPower);

            m_rotationCtrl.SetDirection(m_velocityManager.velocity);

            //�ړI�n�ɓ��B������
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
        /// Ray�̋y�΂Ȃ��ꏊ�̎擾
        /// </summary>
        /// <param name="bread">Bread�f�[�^</param>
        /// <returns></returns>
        private Vector3? CalcuTargetPosition(BreadCrumb bread)
        {
            var positions = bread.GetCopyPositions();

            //�ŐV�̃|�W�V��������Q��
            for (int i = positions.Count - 1; i > 0; i--)
            {
                //���E���̃|�W�V�������擾
                if (m_eye.IsInEyeRange(positions[i]))
                {
                    return positions[i];
                }
            }

            return null;
        }

        #endregion

        #region �A�N�Z�b�T

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
