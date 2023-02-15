using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TesterAttack : MonoBehaviour
{
    [SerializeField]
    private float m_range = 5.0f;

    [SerializeField]
    private Vector3 m_offset = new Vector3(0.0f, 0.25f, 0.0f);

    private void Update()
    {
        if (PlayerInputer.IsShotDown())
        {
            Debug.Log("Åö" + "shot");
            RaycastHit hit;
            if(Physics.Raycast(transform.position + m_offset, transform.forward, out hit, m_range))
            {
                Debug.Log("Åö" + "Hit");
                var damaged = hit.collider.gameObject.GetComponent<I_Damaged>();
                damaged?.Damaged(new DamageData(1.0f, gameObject, hit.collider));
            }
        }
    }
}
