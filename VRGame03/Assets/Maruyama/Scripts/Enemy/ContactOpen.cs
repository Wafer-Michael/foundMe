using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ContactOpen : MonoBehaviour
{
    private void OnCollisionEnter(Collision collision)
    {
        var openDoor = collision.gameObject.GetComponentInParent<OpenDoor>();

        if (openDoor) {
            openDoor.Open(gameObject);
        }
    }
}
