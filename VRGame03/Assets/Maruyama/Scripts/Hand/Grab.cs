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
        //�l�����w�̃s�b�`���O���擾
        isIndexPinching = MYRightHand.GetFingerIsPinching(OVRHand.HandFinger.Index);
        //�e�w�̃s�b�`���O
        ThumbPinchStrength = MYRightHand.GetFingerPinchStrength(OVRHand.HandFinger.Thumb);

        //�l�����w�̃|�W�V����
        Vector3 indexTipPos = MYRightSkelton.Bones[(int)OVRSkeleton.BoneId.Hand_IndexTip].Transform.position;
        //�l�w���w�̃��[�e�[�V����
        Quaternion indexTipRotate = MYRightSkelton.Bones[(int)OVRSkeleton.BoneId.Hand_IndexTip].Transform.rotation;
        //�X�t�B�A���w�̃|�W�V�����Ɖ�]�ɍ��킹��B
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

        //if (ThumbPinchStrength > 0.9f)///����
        //{
        //    other.gameObject.transform.parent = IndexSphere.transform;
        //    other.GetComponent<Rigidbody>().isKinematic = true;
        //    other.gameObject.transform.localPosition = Vector3.zero;
            
        //}
        //else///�͂Ȃ���
        //{
        //    other.GetComponent<Rigidbody>().isKinematic = false;
        //    other.transform.parent = null;

        //}
    }
}