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
        private GameObject m_normalUIs;   //�ʏ펞��UI�z��

        [SerializeField]
        private GameObject m_jackUIs;     //�W���b�N���ɕ\������UI�Q

        private List<Renderer> m_jackUIChildRenders = new List<Renderer>();

        private void Awake()
        {
            //���A�N�e�B�u�v���p�e�B�̒ǉ�
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
