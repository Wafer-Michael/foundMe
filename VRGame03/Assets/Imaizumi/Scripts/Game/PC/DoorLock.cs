using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEditor;

public class DoorLock : MonoBehaviour
{
    int m_digit = 3;

    List<int> m_lockNumbers = new List<int>();

    [SerializeField]
    bool m_isLock;
    public bool IsLock {get;}

    [SerializeField]
    GameObject m_numberText;

    [SerializeField]
    GameObject generator;

    private void Start()
    {
        m_digit = m_numberText.transform.childCount;
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.T))
        {
            AccessKey();
        }
    }

    /// <summary>
    /// �J���O�̏���
    /// </summary>
    void AccessKey()
    {
        DecisionDoorNumber();
        m_isLock = true;
        m_numberText.SetActive(true);
        StartCoroutine("Unlock");
        this.enabled = false;
    }

    /// <summary>
    /// �����J����
    /// </summary>
    IEnumerator Unlock()
    {
        Debug.Log("Start Coroutine");

        List<int> numbers = new List<int>();

        while (m_isLock)
        {
            yield return new WaitWhile(() => !Input.GetKeyDown(KeyCode.Space));
            numbers.Clear(); // �ԍ������Z�b�g
            InputPass(ref numbers); // �ԍ�����
            StartCoroutine("Collation", numbers); // �ƍ�

            yield break;
        }

        Debug.Log("unlocked");

        m_numberText.SetActive(false);
        this.enabled = true;
        yield break;
    }

    /// <summary>
    /// List�ɔԍ�����͂���
    /// </summary>
    /// <param name="numbers">���͂���List</param>
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

    /// <summary>
    /// ���͂����ԍ��������������肷��
    /// </summary>
    /// <param name="numbers">���͂���ԍ�</param>
    IEnumerator Collation(List<int> numbers)
    {
        int correct = 0;    //��v
        int almost = 0;     //�ɂ���

        for (int i = 0; i < numbers.Count; i++)
        {
            if (numbers[i] == m_lockNumbers[i])
            {
                correct += 1;
                numbers[i]  = -1;
            }
            yield return new WaitForEndOfFrame();
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

        if (correct == m_digit) // ��v�������������ƈꏏ�Ȃ�
        {
            m_isLock = false;
            StartCoroutine("Unlock");
            yield break;
        }

        Debug.Log("��v " + correct + "�A ��������v " + almost + "�A �s��v " + (m_digit - correct - almost));

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

    /// <summary>
    /// �Ïؔԍ����擾����
    /// </summary>
    void DecisionDoorNumber()
    {
        var door = FindChildTag(this.gameObject, "Door");
        var doorTex = FetchTextureName(door); // �h�A�̃e�N�X�`��

        var wall = FindChildTag(this.gameObject.transform.parent.gameObject, "Wall"); // ��
        var wallTex = FetchTextureName(wall); // �ǂ̃e�N�X�`��    

        List<int> numbers = new List<int>();

        // �ԍ����擾
        var numbergene = generator.GetComponent<NumberLockGenerator>();
        numbers.Add(numbergene.FetchNumber(wallTex, NumberLockGenerator.NumberType.WallPattern));
        numbers.Add(numbergene.FetchNumber(wallTex, NumberLockGenerator.NumberType.WallColor));
        numbers.Add(numbergene.FetchNumber(doorTex, NumberLockGenerator.NumberType.DoorColor));

        Debug.Log("lock Number " + numbers[0] + numbers[1] + numbers[2]);

        SetLockNumbers(numbers);
    }

    /// <summary>
    /// �^�O�������Ă���q�I�u�W�F�N�g���擾����
    /// </summary>
    /// <param name="parentObj">�擾�������I�u�W�F�N�g�̐e�I�u�W�F�N�g</param>
    /// <param name="tag">�擾�������I�u�W�F�N�g�̃^�O</param>
    /// <returns>�q�I�u�W�F�N�g</returns>
    GameObject FindChildTag(GameObject parentObj, string tag)
    {
        GameObject result = null;

        var numChild = parentObj.transform.childCount; // �q�I�u�W�F�N�g�̐�
        for (int i = 0; i < numChild; i++)
        {
            var child = parentObj.transform.GetChild(i).gameObject;
            if (child.tag == tag)
            {
                result = child;
            }
        }

        return result;
    }

    /// <summary>
    /// �I�u�W�F�N�g���g�p���Ă���e�N�X�`�����擾����
    /// </summary>
    /// <param name="gameObj">��������I�u�W�F�N�g</param>
    /// <returns>�g�p���Ă���e�N�X�`��</returns>
    Texture FetchTextureName(GameObject gameObj)
    {
        Texture result = null; // �擾�����e�N�X�`��

        var mat = gameObj.GetComponent<Renderer>().material; // �I�u�W�F�N�g�̃}�e���A��
        var shader = mat.shader; // �}�e���A�����g�p���Ă���V�F�[�_�[

        var count = ShaderUtil.GetPropertyCount(shader);
        for (int i = 0; i < count; i++)
        {
            var type = ShaderUtil.GetPropertyType(shader, i);
            if (type == ShaderUtil.ShaderPropertyType.TexEnv)
            {
                var proName = ShaderUtil.GetPropertyName(shader, i);
                var tex = mat.GetTexture(proName);
                if (tex)
                {
                    result = tex;
                    Debug.Log(result.name);
                }
            }
        }

        return result;
    }
}