using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Title : MonoBehaviour
{
    [SerializeField]
    string m_sceneName;

    [SerializeField]
    KeyCode m_keyCode;

    void Update()
    {
        if (Input.GetKeyDown(m_keyCode))
        {
            ChangeScene();
        }
    }

    void ChangeScene()
    {
        SceneManager.LoadScene(m_sceneName);
    }
}
