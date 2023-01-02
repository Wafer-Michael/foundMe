using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Tuple
{

    //--------------------------------------------------------------------------------------
    /// �^�v���X�y�[�X���g�p�����
    //--------------------------------------------------------------------------------------
    public interface I_Tupler
    {
        public TupleSpace GetTupleSpace();
    }

    //--------------------------------------------------------------------------------------
    /// �^�v���̃C���^�[�t�F�[�X
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
    /// �^�v���X�y�[�X
    //--------------------------------------------------------------------------------------
    public class TupleSpace
    {
        Dictionary<System.Type, List<I_Tuple>> m_tuplesMap;           //�������܂ꂽ���ꗗ

        Dictionary<System.Type, List<I_NotifyController>> m_notifysMap; //Notify�f�[�^�̃��X�g

        public TupleSpace()
        {
            m_tuplesMap = new Dictionary<System.Type, List<I_Tuple>>();
            m_notifysMap = new Dictionary<System.Type, List<I_NotifyController>>();
        }

        //--------------------------------------------------------------------------------------
        /// �A�N�Z�b�T
        //--------------------------------------------------------------------------------------

        public bool IsSomeTuple<TupleType>(TupleType newTuple)
            where TupleType : TupleBase
        {
            var type = typeof(TupleType);

            //�L�[�����݂��Ȃ��Ȃ�
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
            if (!m_notifysMap.ContainsKey(type)) {  //Notify���o�^����ĂȂ�������
                return false;   //�����ʒm���Ȃ�
            }

            var notifys = m_notifysMap[type];

            foreach(var notify in notifys)
            {
                //�����ʒm�Ȃ�
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

            if(IsSomeTuple<TupleType>(newTuple)) {  //�����^�v���Ȃ珑�����܂Ȃ�
                return;
            }

            if (m_tuplesMap.ContainsKey(type)) {    //���݂��Ȃ��Ȃ�z��̃��������m��
                m_tuplesMap[type] = new List<I_Tuple>();
            }

            m_tuplesMap[type].Add(newTuple);    //�^�v���̓o�^

            CallNotifys<TupleType>(newTuple);    //�o�^���ꂽ�ʒm���Ăяo���B
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

            if (!m_notifysMap.ContainsKey(type)) {  //�L�[�����݂��Ȃ��Ȃ�
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
                //�����I�ɏ�����ς���
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

            if (!m_tuplesMap.ContainsKey(type)) {   //�L�[�����݂��Ȃ��Ȃ�
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


