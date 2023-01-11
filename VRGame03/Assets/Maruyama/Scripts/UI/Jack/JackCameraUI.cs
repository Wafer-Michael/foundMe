using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JackCameraUI : MonoBehaviour
{
    [SerializeField]
    private FadeObject m_fadeComponent;

    public void FadeStart(FadeObject.FadeType fadeType)
    {
        m_fadeComponent.FadeStart(fadeType);
    }
}
