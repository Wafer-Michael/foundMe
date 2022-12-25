using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SubBehavior
{
    public abstract class SubBehaviorTreeBase<EnumType>
        where EnumType : System.Enum
    {
        private GameObject m_owner;                         //�I�[�i�[�I�u�W�F�N�g

        protected BehaviorTree<EnumType> m_behaviorTree;    //�r�w�C�r�A�c���[

        public SubBehaviorTreeBase(GameObject owner)
        {
            m_owner = owner;
        }

        public void OnCreate()
        {
            CreateNode();
            CreateEdge();
            CreateDecorator();
        }

        public void OnUpdate()
        {
            m_behaviorTree.OnUpdate();
        }

        protected abstract void CreateNode();
        protected abstract void CreateEdge();
        protected abstract void CreateDecorator();

        //--------------------------------------------------------------------------------------
        /// �A�N�Z�b�T
        //--------------------------------------------------------------------------------------

        public GameObject GetOwner() { return m_owner; }

        /// <summary>
        /// �r�w�C�r�A�̋����I��
        /// </summary>
        public void ForceStop() { m_behaviorTree.ForceStop(); }
    }

}



