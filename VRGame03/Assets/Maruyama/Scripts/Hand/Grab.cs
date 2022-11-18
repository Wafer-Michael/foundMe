using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Grab : MonoBehaviour
{
    [SerializeField] OVRHand MYRightHand;
    [SerializeField] OVRSkeleton MYRightSkelton;
    [SerializeField] GameObject IndexSphere;
    private bool isIndexPinching;
    private float ThumbPinchStrength;

    void Update()
    {
        //人差し指のピッチングを取得
        isIndexPinching = MYRightHand.GetFingerIsPinching(OVRHand.HandFinger.Index);
        //親指のピッチング
        ThumbPinchStrength = MYRightHand.GetFingerPinchStrength(OVRHand.HandFinger.Thumb);

        //人差し指のポジション
        Vector3 indexTipPos = MYRightSkelton.Bones[(int)OVRSkeleton.BoneId.Hand_IndexTip].Transform.position;
        //人指し指のローテーション
        Quaternion indexTipRotate = MYRightSkelton.Bones[(int)OVRSkeleton.BoneId.Hand_IndexTip].Transform.rotation;
        //スフィアを指のポジションと回転に合わせる。
        IndexSphere.transform.position = indexTipPos;
        IndexSphere.transform.rotation = indexTipRotate;
    }

    private void OnCollisionEnter(Collision collision)
    {
        //collision.gameObject.GetComponent<Renderer>().material.color = Color.blue;
    }

    private void OnTriggerEnter(Collider other)
    {
        //other.gameObject.GetComponent<Renderer>().material.color = Color.green;
    }

    void OnTriggerStay(Collider other)
    {
        //other.GetComponent<Renderer>().material.color = Color.red;

        //if (ThumbPinchStrength > 0.9f)///つかんだ
        //{
        //    other.gameObject.transform.parent = IndexSphere.transform;
        //    other.GetComponent<Rigidbody>().isKinematic = true;
        //    other.gameObject.transform.localPosition = Vector3.zero;
            
        //}
        //else///はなした
        //{
        //    other.GetComponent<Rigidbody>().isKinematic = false;
        //    other.transform.parent = null;

        //}
    }
}