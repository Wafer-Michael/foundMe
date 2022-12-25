using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SubBehavior
{
    public abstract class SubBehaviorTreeBase<EnumType>
        where EnumType : System.Enum
    {
        private GameObject m_owner;                         //オーナーオブジェクト

        protected BehaviorTree<EnumType> m_behaviorTree;    //ビヘイビアツリー

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
        /// アクセッサ
        //--------------------------------------------------------------------------------------

        public GameObject GetOwner() { return m_owner; }

        /// <summary>
        /// ビヘイビアの強制終了
        /// </summary>
        public void ForceStop() { m_behaviorTree.ForceStop(); }
    }

}



