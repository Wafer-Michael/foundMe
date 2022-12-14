using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UniRx;

namespace UIObserver {
    public class JackUIObserver : MonoBehaviour
    {
        [SerializeField]
        private HijackController m_jackController;

        [SerializeField]
        private GameObject m_normalUIs;   //通常時のUI配列

        [SerializeField]
        private GameObject m_jackUIs;     //ジャック中に表示するUI群

        private List<Renderer> m_jackUIChildRenders = new List<Renderer>();

        private void Awake()
        {
            //リアクティブプロパティの追加
            m_jackController.IsJackObserver.
                Subscribe(isJack => ChangeUI(isJack)).
                AddTo(this);

            var childMembers = m_jackUIs.GetComponentsInChildren<Renderer>();  //JackUIs
            m_jackUIChildRenders = new List<Renderer>(childMembers);

            foreach (var child in m_jackUIChildRenders)
            {
                child.enabled = false;
            }
        }

        private void ChangeUI(bool isJack)
        {
            //NormalUIs.SetActive(!isJack);
            //m_jackUIs.SetActive(isJack);

            foreach (var child in m_jackUIChildRenders)
            {
                child.enabled = isJack;
            }
        }

        public void TouchReturnBottuon(OculusSampleFramework.InteractableStateArgs obj)
        {
            m_jackController.ForceCamBack();
        }
    }
}
