using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Tuple
{

    //--------------------------------------------------------------------------------------
    /// �ʒm�p�f�[�^�Ǘ��C���^�[�t�F�[�X
    //--------------------------------------------------------------------------------------
    public interface I_NotifyController
    {
        public I_Tupler GetRequester();

        public bool IsEqual(I_NotifyController other);
    }

    //--------------------------------------------------------------------------------------
    /// �ʒm�p�f�[�^�Ǘ�
    //--------------------------------------------------------------------------------------
    public class NotifyController<TupleType> : I_NotifyController
        where TupleType : I_Tuple
    {
        private I_Tupler m_requester;                      //�ʒm�o�^��
        private System.Action<TupleType> m_function;       //�Ăяo����������
        private System.Func<TupleType, bool> m_isCool;     //�Ăяo������

        public NotifyController(
            I_Tupler requester,
            System.Action<TupleType> function
        ) :
            this(requester, function, (TupleType tuple) => { return true; })
        { }

        public NotifyController(
            I_Tupler requester,
            System.Action<TupleType> function,
            System.Func<TupleType, bool> isCool
        )
        {
            this.m_requester = requester;
            this.m_function = function;
            this.m_isCool = isCool;
        }

        public void Invoke(TupleType tuple)
        {
            if (m_isCool.Invoke(tuple))
            {
                m_function(tuple);
            }
        }

        public I_Tupler GetRequester() { return m_requester; }

        /// <summary>
        /// �������ǂ���(�����I�ɏ�����ς���)
        /// </summary>
        /// <param name="other">��r�Ώ�</param>
        /// <returns>�����Ȃ�true</returns>
        public bool IsEqual(I_NotifyController other) { return this.GetRequester() == other.GetRequester(); }
    }
}
