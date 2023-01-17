using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DoorLock : MonoBehaviour
{
    const int m_digit = 3;

    int[] m_lockNumbers = new int[m_digit];

    [SerializeField]
    bool m_isLock;

    [SerializeField]
    GameObject m_numberText;

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.T))
        {
            Debug.Log("locked");
            m_isLock = true;
            AccessKey();
        }
    }

    public void AccessKey()
    {
        MakePass();
        StartCoroutine("Unlock");
    }

    void MakePass()
    {
        for (int i = 0; i < m_digit; i++)
        {
            m_lockNumbers[i] = (int)Random.Range(0, 9);
        }

        Debug.Log("Make Pass  " + m_lockNumbers[0] + m_lockNumbers[1] + m_lockNumbers[2]);
    }

    IEnumerator Unlock()
    {
        Debug.Log("Start Coroutine");

        int[] numbers = new int[m_digit];

        while (m_isLock)
        {
            yield return new WaitWhile(() => !Input.GetKeyDown(KeyCode.Space));
            //if (Input.GetKey(KeyCode.Space))
            //{
                InputPass(ref numbers);
                StartCoroutine("Collation", numbers);
            //}
            yield return new WaitForEndOfFrame();
        }

        Debug.Log("unlocked");

        yield break;
    }

    void InputPass(ref int[] numbers)
    {
        for (int i = 0; i < m_numberText.transform.childCount; i++)
        {
            var numberText = m_numberText.transform.GetChild(i).gameObject;

            int textNum = int.Parse(numberText.GetComponent<Text>().text);
            numbers[i] = textNum;
        }

        Debug.Log("input number" + numbers[0] + numbers[1] + numbers[2]);
    }

    IEnumerator Collation(int[] numbers)
    {
        int correct = 0;
        int almost = 0;

        for (int i = 0; i < numbers.Length; i++)
        {
            for (int j = 0; j < m_lockNumbers.Length; j++)
            {
                if (numbers[i] == m_lockNumbers[j])
                {
                    if (i == j)
                    {
                        correct++;
                    }
                    else
                    {
                        almost++;
                    }
                    break;
                }

                yield return new WaitForEndOfFrame();
            }
        }

        if (correct == m_digit)
        {
            m_isLock = false;
            yield break;
        }

        Debug.Log("ˆê’v " + correct + "A ”Žš‚ªˆê’v " + almost + "A •sˆê’v " + (m_digit - correct - almost));


        yield break;
    }
}
