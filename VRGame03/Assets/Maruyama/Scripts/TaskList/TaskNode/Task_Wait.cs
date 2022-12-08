using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using System;
using UnityEngine.Events;

namespace TaskNode
{
    public class Task_Wait : TaskNodeBase
    {
        #region �p�����[�^

        [Serializable]
        public struct Parametor
        {
            public float time;
            public Action enter;
            public Action update;
            public Action exit;

            public Parametor(float time)
            {
                this.time = time;
                enter = null;
                update = null;
                exit = null;
            }

            public Parametor(float time, Action enter, Action update, Action exit)
            {
                this.time = time;
                this.enter = enter;
                this.update = update;
                this.exit = exit;
            }
        }

        #endregion

        #region �����o�ϐ�

        private GameTimer m_timer = new GameTimer();

        private Parametor m_param = new Parametor();

        #endregion

        #region �R���X�g���N�^

        public Task_Wait(float time)
        {
            m_param = new Parametor(time);
        }
        public Task_Wait(Parametor param)
        {
            m_param = param;
        }

        #endregion

        #region Enter,Update,Exit

        public override void OnEnter()
        {
            m_param.enter?.Invoke();

            m_timer.ResetTimer(m_param.time);
        }

        public override bool OnUpdate()
        {
            m_timer.UpdateTimer();
            m_param.update?.Invoke();

            return m_timer.IsTimeUp;
        }

        public override void OnExit()
        {
            m_param.exit?.Invoke();
        }

        #endregion
    }
}
