using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using OculusSampleFramework;

public interface I_VRUI
{
    public void Open();

    public void Close();

    public void Touch(InteractableStateArgs obj);
}
