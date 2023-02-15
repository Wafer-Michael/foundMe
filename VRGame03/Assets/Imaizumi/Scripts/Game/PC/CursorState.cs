using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CursorState : MonoBehaviour
{
    void Start()
    {
        Screen.fullScreen = true;

        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }
}
