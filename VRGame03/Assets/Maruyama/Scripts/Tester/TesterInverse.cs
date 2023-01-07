using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TesterInverse : MonoBehaviour
{
    [SerializeField]
    private GameObject m_target;

    void Update()
    {
        if (PlayerInputer.IsShotDown())
        {
            if(m_target == null) {
                return;
            }

            Debug.Log("��" + transform.InverseTransformPoint(m_target.transform.position));
            Debug.Log("����" + maru.Utility.InverseTransformPoint(transform.position, transform.rotation, m_target.transform.position));
        }
    }
}
