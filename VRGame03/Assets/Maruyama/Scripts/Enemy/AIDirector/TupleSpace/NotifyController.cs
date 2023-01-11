using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Tuple
{

    //--------------------------------------------------------------------------------------
    /// 通知用データ管理インターフェース
    //--------------------------------------------------------------------------------------
    public interface I_NotifyController
    {
        public I_Tupler GetRequester();

        public bool IsEqual(I_NotifyController other);
    }

    //--------------------------------------------------------------------------------------
    /// 通知用データ管理
    //--------------------------------------------------------------------------------------
    public class NotifyController<TupleType> : I_NotifyController
        where TupleType : I_Tuple
    {
        private I_Tupler m_requester;                      //通知登録者
        private System.Action<TupleType> m_function;       //呼び出したい処理
        private System.Func<TupleType, bool> m_isCool;     //呼び出し条件

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
        /// 同じかどうか(将来的に条件を変える)
        /// </summary>
        /// <param name="other">比較対象</param>
        /// <returns>同じならtrue</returns>
        public bool IsEqual(I_NotifyController other) { return this.GetRequester() == other.GetRequester(); }
    }
}
