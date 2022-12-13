using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace UIObserver {
    public class JackUIObserver : MonoBehaviour
    {
        [SerializeField]
        private HijackController m_jackController;

        [SerializeField]
        private GameObject NormalUIs;   //通常時のUI配列

        [SerializeField]
        private GameObject JackUIs;     //ジャック中に表示するUI群

        private void Update()
        {
            
        }
    }
}
