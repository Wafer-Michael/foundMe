using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TriggetTest : MonoBehaviour
{
    [SerializeField] OVRHand MYRightHand;
    [SerializeField] OVRSkeleton MYRightSkelton;
    [SerializeField] GameObject IndexSphere;
    private bool isIndexPinching;
    private float ThumbPinchStrength;

    private void Update()
    {
        
    }

    private void OnTriggerStay(Collider other)
    {
        //if (ThumbPinchStrength > 0.9f)///‚Â‚©‚ñ‚¾
        //{
        //    other.gameObject.transform.parent = IndexSphere.transform;
        //    other.GetComponent<Rigidbody>().isKinematic = true;
        //    other.gameObject.transform.localPosition = Vector3.zero;

        //}
        //else///‚Í‚È‚µ‚½
        //{
        //    other.GetComponent<Rigidbody>().isKinematic = false;
        //    other.transform.parent = null;

        //}
    }
}
