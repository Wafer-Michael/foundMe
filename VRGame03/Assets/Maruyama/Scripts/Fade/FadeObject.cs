using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class FadeObject : MonoBehaviour
{
    public enum FadeType
    {
        FadeOut,
        FadeIn
    }

    public abstract void FadeStart();

    public abstract void FadeStart(FadeType type);

    public abstract bool IsFinish();
}
