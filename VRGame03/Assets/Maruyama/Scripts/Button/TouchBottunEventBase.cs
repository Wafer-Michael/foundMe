using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using OculusSampleFramework;

public abstract class TouchBottunEventBase : MonoBehaviour
{
    public abstract void Touch(InteractableStateArgs obj); 
}
