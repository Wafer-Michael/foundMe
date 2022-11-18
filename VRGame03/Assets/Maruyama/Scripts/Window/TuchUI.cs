using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TuchUI : MonoBehaviour
{
    [SerializeField]
    private OVRHand hand;

    [SerializeField]
    private Renderer renderer;

    void Start()
    {
        //renderer = GameObject.Find("Cube").GetComponent<Renderer>();
        //hand = this.gameObject.GetComponent<OVRHand>();

        //renderer = GetComponent<Renderer>();
    }

    void Update()
    {
        

        //renderer.material.color = Color.blue;
        //if (hand.GetFingerIsPinching(OVRHand.HandFinger.Middle))
        //{
        //    renderer.material.color = Color.blue;
        //}
        //else if (renderer.material.color == Color.blue)
        //{
        //    renderer.material.color = Color.white;
        //}
    }


    private void OnTriggerEnter(Collider other)
    {
        renderer.material.color = Color.red;

        if (other.gameObject.name == "Hand_Index3_CapsuleCollider")
        {
            renderer.material.color = Color.red;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.name == "Hand_Index3_CapsuleCollider")
        {
            renderer.material.color = Color.white;
        }
    }


}
