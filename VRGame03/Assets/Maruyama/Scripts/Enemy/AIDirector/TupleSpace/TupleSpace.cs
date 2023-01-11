using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Tuple
{

    //--------------------------------------------------------------------------------------
    /// タプルスペースを使用する者
    //--------------------------------------------------------------------------------------
    public interface I_Tupler
    {
        public TupleSpace GetTupleSpace();
    }

    //--------------------------------------------------------------------------------------
    /// タプルのインターフェース
    //--------------------------------------------------------------------------------------
    public interface I_Tuple
    {
        public I_Tupler GetRequester();

        public bool IsEqual<T>(T other);
    }

    public abstract class TupleBase : I_Tuple
    {
        public abstract I_Tupler GetRequester();

        public abstract bool IsEqual<T>(T other);
    }


    //--------------------------------------------------------------------------------------
    /// タプルスペース
    //--------------------------------------------------------------------------------------
    public class TupleSpace
    {
        Dictionary<System.Type, List<I_Tuple>> m_tuplesMap;           //書き込まれた情報一覧

        Dictionary<System.Type, List<I_NotifyController>> m_notifysMap; //Notifyデータのリスト

        public TupleSpace()
        {
            m_tuplesMap = new Dictionary<System.Type, List<I_Tuple>>();
            m_notifysMap = new Dictionary<System.Type, List<I_NotifyController>>();
        }

        //--------------------------------------------------------------------------------------
        /// アクセッサ
        //--------------------------------------------------------------------------------------

        public bool IsSomeTuple<TupleType>(TupleType newTuple)
            where TupleType : TupleBase
        {
            var type = typeof(TupleType);

            //キーが存在しないなら
            if (!m_tuplesMap.ContainsKey(type)) {
                return false;
            }

            foreach(var tuple in m_tuplesMap[type])
            {
                var tTuple = (TupleType)tuple;
                
                if(tTuple.IsEqual(newTuple)) {
                    return true;
                }
            }

            return false;
        }

        public bool IsSomeNotify<NotifyType>(I_NotifyController newNotify)
        {
            var type = typeof(NotifyType);
            if (!m_notifysMap.ContainsKey(type)) {  //Notifyが登録されてなかったら
                return false;   //同じ通知がない
            }

            var notifys = m_notifysMap[type];

            foreach(var notify in notifys)
            {
                //同じ通知なら
                if (notify.IsEqual(newNotify)) {
                    return true;
                }
            }

            return false;
        }

        public void Write<TupleType>(TupleType newTuple)
            where TupleType : TupleBase
        {
            var type = typeof(TupleType);

            if(IsSomeTuple<TupleType>(newTuple)) {  //同じタプルなら書き込まない
                return;
            }

            if (m_tuplesMap.ContainsKey(type)) {    //存在しないなら配列のメモリを確保
                m_tuplesMap[type] = new List<I_Tuple>();
            }

            m_tuplesMap[type].Add(newTuple);    //タプルの登録

            CallNotifys<TupleType>(newTuple);    //登録された通知を呼び出す。
        }

        public TupleType Read<TupleType>(System.Func<TupleType, bool> isSearch)
            where TupleType : TupleBase
        {
            return SearchTuple<TupleType>(isSearch);
        }

        public TupleType Take<TupleType>(System.Func<TupleType, bool> isSearch)
            where TupleType : TupleBase
        {
            var tuple = SearchTuple<TupleType>(isSearch);
            if (tuple != null) {
                return null;
            }

            RemoveTuple<TupleType>(tuple);

            return tuple;
        }

        public void Take<TupleType>(TupleType takeTuple)
            where TupleType : TupleBase
        {
            var type = typeof(TupleType);

            if (!m_tuplesMap.ContainsKey(type)) {
                return;
            }

            var tuples = m_tuplesMap[type];
            foreach(var tuple in tuples)
            {
                if (tuple.IsEqual<TupleType>(takeTuple))
                {
                    RemoveTuple<TupleType>(takeTuple);
                }
            }            
        }

        public void Notify<TupleType>(
            I_Tupler requester,
            System.Action<TupleType> function,
            System.Func<TupleType, bool> isCall
        ) 
            where TupleType : TupleBase, new()
        {
            var type = typeof(TupleType);

            var newNotify = new NotifyController<TupleType>(requester, function, isCall);

            if (IsSomeNotify<TupleType>(newNotify)) {
                return;
            }

            if (!m_notifysMap.ContainsKey(type)) {  //キーが存在しないなら
                m_notifysMap[type] = new List<I_NotifyController>();
            }

            m_notifysMap[type].Add(newNotify);
        }

        public bool RemoveNotify<TupleType>(I_Tupler requester)
            where TupleType : TupleBase
        {
            if(requester == null) {
                return false;
            }

            var type = typeof(TupleType);

            if (!m_notifysMap.ContainsKey(type)) {
                return false;
            }

            var notifys = m_notifysMap[type];
            foreach(var notify in notifys)
            {
                //将来的に条件を変える
                if(notify.GetRequester() == requester)
                {
                    return notifys.Remove(notify);
                }
            }

            return false;
        }

        public void RemoveAllNotifys(I_Tupler requester)
        {
            //System.Action removeFunc;

            foreach(var pair in m_notifysMap)
            {
                foreach(var notify in pair.Value)
                {
                    if(notify.GetRequester() == requester)
                    {
                        pair.Value.Remove(notify);
                    }
                }
            }
        }

        public void RemoveAllTuples(I_Tupler requester)
        {
            foreach(var pair in m_tuplesMap)
            {
                foreach(var tuple in pair.Value)
                {
                    pair.Value.Remove(tuple);
                }
            }
        }
            
        private void CallNotifys<TupleType>(TupleType tuple)
            where TupleType : TupleBase
        {
            var type = typeof(TupleType);
            if (!m_notifysMap.ContainsKey(type)) {
                return;
            }

            var notifys = m_notifysMap[type];

            foreach(var i_notify in notifys)
            {
                var notify = (NotifyController<TupleType>)(i_notify);

                notify.Invoke(tuple);
            }
        }

        private TupleType SearchTuple<TupleType>(System.Func<TupleType, bool> isSearch)
            where TupleType : TupleBase
        {
            var type = typeof(TupleType);

            if (!m_tuplesMap.ContainsKey(type)) {   //キーが存在しないなら
                return null;
            }

            var tuples = m_tuplesMap[type];
            foreach(var i_tuple in tuples)
            {
                var tuple = (TupleType)(i_tuple);

                if (isSearch(tuple)) {
                    return tuple;
                }
            }

            return null;
        }

        private bool RemoveTuple<TupleType>(TupleType removeTuple)
            where TupleType : TupleBase
        {
            var type = typeof(TupleType);

            if (!m_tuplesMap.ContainsKey(type)) {
                return false;
            }

            var tuples = m_tuplesMap[type];
            return tuples.Remove(removeTuple);
        }

        private bool RemoveNotify(I_NotifyController removeNotify)
        {
            foreach(var pair in m_notifysMap)
            {
                bool isRemove = pair.Value.Remove(removeNotify);
                if (isRemove) {
                    return isRemove;
                }
            }

            return false;
        }

    }

}


