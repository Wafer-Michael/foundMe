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

    [SerializeField]
    GameObject m_lights;

    [SerializeField]
    GameObject m_canvas;

    [SerializeField]
    float m_lightOffTime;


    private void Start()
    {
        StartCoroutine("StartDirection");
    }

    void Update()
    {
        if(m_lights.transform.childCount <= 0)
        {
            m_canvas.SetActive(false);
            ChangeScene();
        }
    }

    IEnumerator StartDirection()
    {
        yield return new WaitWhile(() => !Input.GetKeyDown(m_keyCode));

        while(m_lights.transform.childCount >= 0)
        {
            yield return new WaitForSeconds(m_lightOffTime);

            var child = m_lights.transform.GetChild(0).gameObject;
            if(child)
            {
                Destroy(child);
            }
        }

        yield break;
    }

    void ChangeScene()
    {
        SceneManager.LoadScene(m_sceneName);
    }
}
