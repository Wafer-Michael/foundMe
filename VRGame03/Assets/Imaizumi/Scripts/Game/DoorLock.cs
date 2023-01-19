using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DoorLock : MonoBehaviour
{
    int m_digit = 3;

    List<int> m_lockNumbers = new List<int>();

    [SerializeField]
    bool m_isLock;
    public bool IsLock {get;}

    [SerializeField]
    GameObject m_numberText;

    private void Start()
    {
        m_digit = m_numberText.transform.childCount;
    }

    void Update()
    {
        Debug.Log(m_isLock);
        if (Input.GetKeyDown(KeyCode.T))
        {
            Debug.Log("locked");
            m_isLock = true;
            StartCoroutine("Unlock");
            this.enabled = false;
        }
    }

    IEnumerator Unlock()
    {
        Debug.Log("Start Coroutine");

        List<int> numbers = new List<int>();

        while (m_isLock)
        {
            yield return new WaitWhile(() => !Input.GetKeyDown(KeyCode.Space));
            numbers.Clear();
            InputPass(ref numbers);
            StartCoroutine("Collation", numbers);

            yield break;
        }

        Debug.Log("unlocked");

        this.enabled = true;
        yield break;
    }

    void InputPass(ref List<int> numbers)
    {
        for (int i = 0; i < m_digit; i++)
        {
            var numberText = m_numberText.transform.GetChild(i).gameObject;

            int textNum = int.Parse(numberText.GetComponent<Text>().text);
            numbers.Add(textNum);
        }

        Debug.Log("input number" + numbers[0] + numbers[1] + numbers[2]);
    }

    IEnumerator Collation(List<int> numbers)
    {
        int correct = 0;    //àÍív
        int almost = 0;     //ê…ÇµÇ¢

        foreach(int num in numbers)
        {
            Debug.Log("number " + num);
        }

        for (int i = 0; i < numbers.Count; i++)
        {
            if (numbers[i] == m_lockNumbers[i])
            {
                correct += 1;
                numbers[i]  = -1;
            }
            yield return new WaitForEndOfFrame();
        }

        foreach (int num in numbers)
        {
            Debug.Log("number " + num);
        }

        for (int i = 0; i < numbers.Count; i++)
        {
            for (int j = 0; j < m_lockNumbers.Count; j++)
            {
                if (numbers[i] == m_lockNumbers[j])
                {
                    almost++;
                    break;
                }
                yield return new WaitForEndOfFrame();
            }
        }

        Debug.Log(correct);

        if (correct == m_digit)
        {
            m_isLock = false;
            StartCoroutine("Unlock");
            yield break;
        }

        Debug.Log("àÍív " + correct + "ÅA êîéöÇ™àÍív " + almost + "ÅA ïsàÍív " + (m_digit - correct - almost));

        StartCoroutine("Unlock");
        yield break;
    }

    public void SetLockNumbers(List<int> numbers)
    {
        foreach(int num in numbers)
        {
            m_lockNumbers.Add(num);
        }
    }
}
