using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerPC : MonoBehaviour
{
    [SerializeField]
    float moveSpeed = 5.0f;

    [SerializeField]
    float rotationSpeed = 360.0f;

    [SerializeField]
    Camera camera;
    Rigidbody rigid;

    Animator animator;
    void Start()
    {
        rigid = this.GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 forward = new Vector3(0.0f, 0.0f, 0.0f);

        Vector3 direction = new Vector3(Input.GetAxis("Horizontal"), 0, Input.GetAxis("Vertical"));
        if (direction.sqrMagnitude > 0.01f)
        {
            forward = Vector3.Slerp(transform.forward, direction, rotationSpeed);
        }

        direction = maru.Utility.ConvartCameraVec(direction, camera, this.gameObject);
        rigid.AddForce(direction * moveSpeed);
    }
}
