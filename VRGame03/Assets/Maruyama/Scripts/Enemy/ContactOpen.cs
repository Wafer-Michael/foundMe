using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ContactOpen : MonoBehaviour
{
    private void OnCollisionEnter(Collision collision)
    {
        Debug.Log("ÅöCollision");
        var openDoor = collision.gameObject.GetComponentInParent<OpenDoor>();

        if (openDoor) {
            Debug.Log("ÅöOpen");
            openDoor.Open(gameObject);
        }
    }
}
