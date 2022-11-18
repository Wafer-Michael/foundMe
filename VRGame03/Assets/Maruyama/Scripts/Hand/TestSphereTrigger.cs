using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestSphereTrigger : MonoBehaviour
{
    [SerializeField] OVRHand MYRightHand;
    [SerializeField] OVRSkeleton MYRightSkelton;
    private bool isIndexPinching;
    private float ThumbPinchStrength;

    [SerializeField]
    TMPro.TextMeshProUGUI m_debugText;

    private void Update()
    {
        isIndexPinching = MYRightHand.GetFingerIsPinching(OVRHand.HandFinger.Index);

        ThumbPinchStrength = MYRightHand.GetFingerPinchStrength(OVRHand.HandFinger.Thumb);
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.gameObject == MYRightHand.gameObject)
        {
            return;
        }

        m_debugText.text = "Stay";

        if (ThumbPinchStrength > 0.7f)///‚Â‚©‚ñ‚¾
        {
            m_debugText.text = "Grap";
            //other.gameObject.transform.parent = gameObject.transform;
            //other.GetComponent<Rigidbody>().isKinematic = true;
            //other.gameObject.transform.localPosition = Vector3.zero;
            other.gameObject.GetComponent<Renderer>().material.color = Color.green;
        }
        else///‚Í‚È‚µ‚½
        {
            m_debugText.text = "Hanasita";
            other.GetComponent<Rigidbody>().isKinematic = false;
            other.transform.parent = null;
            other.gameObject.GetComponent<Renderer>().material.color = Color.blue;
        }
    }
}
