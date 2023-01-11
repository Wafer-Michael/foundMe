using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Oculus.Interaction;

public class UITouchMover_Ex : MonoBehaviour
{
    public void Touch_Mover(PointerEvent pointerEvent)
    {
        transform.position = pointerEvent.Pose.position;
    }
}
