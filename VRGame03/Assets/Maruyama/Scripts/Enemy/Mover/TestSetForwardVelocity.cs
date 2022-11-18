using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestSetForwardVelocity : MonoBehaviour
{
    /// <summary>
    /// •ûŒü
    /// </summary>
    public Vector3 Direction { get; set; }

    void Start()
    {
        Direction = transform.forward;
    }

    void Update()
    {
        Debug.Log(Direction);
        Camera.main.transform.forward = Direction;
    }
}
