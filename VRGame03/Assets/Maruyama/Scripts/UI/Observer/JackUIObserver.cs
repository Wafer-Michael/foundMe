using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace UIObserver {
    public class JackUIObserver : MonoBehaviour
    {
        [SerializeField]
        private HijackController m_jackController;

        [SerializeField]
        private GameObject NormalUIs;   //�ʏ펞��UI�z��

        [SerializeField]
        private GameObject JackUIs;     //�W���b�N���ɕ\������UI�Q

        private void Update()
        {
            
        }
    }
}
