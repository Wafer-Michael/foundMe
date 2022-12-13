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
        private GameObject NormalUIs;   //�ʏ펞��UI�z��

        [SerializeField]
        private GameObject JackUIs;     //�W���b�N���ɕ\������UI�Q

        private void Awake()
        {
            //���A�N�e�B�u�v���p�e�B�̒ǉ�
            m_jackController.IsJackObserver.
                Subscribe(isJack => ChangeUI(isJack)).
                AddTo(this);
        }

        private void ChangeUI(bool isJack)
        {
            //NormalUIs.SetActive(!isJack);
            JackUIs.SetActive(isJack);
        }

        public void TouchReturnBottuon(OculusSampleFramework.InteractableStateArgs obj)
        {
            m_jackController.ForceCamBack();
        }
    }
}
