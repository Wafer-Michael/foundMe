using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SenceController : MonoBehaviour
{
    static string[] DEFAULT_NORMAL_LAYERS = new string[] { "L_Obstacle" };
    static string[] DEFAULT_SENCE_LAYERS = new string[] { "L_Obstacle" };

    [SerializeField]
    private Camera m_camera;

    [SerializeField]
    private string[] m_nomalEyeLayerStrings = DEFAULT_NORMAL_LAYERS;

    [SerializeField]
    private string[] m_senceEyeLayerStrings = DEFAULT_SENCE_LAYERS;

    private void Update()
    {
           
    }

    private void StartSence()
    {
        //m_camera.cullingMask = ;
    }
}
