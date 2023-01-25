using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ContactOpen : MonoBehaviour
{
    private void OnCollisionEnter(Collision collision)
    {
        Debug.Log("��Collision");
        var openDoor = collision.gameObject.GetComponentInParent<OpenDoor>();

        if (openDoor) {
            Debug.Log("��Open");
            openDoor.Open(gameObject);
        }
    }
}
