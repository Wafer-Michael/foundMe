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
    GameObject m_canvas;
    GameObject m_numberText;

    GameObject m_generator;

    int[] m_collationNumbers = new int[3];

    int m_correct = 0;    //��v
    int m_almost = 0;     //�ɂ���

    private void Awake()
    {
        m_generator = GameObject.Find("NumberLockGenerator");
    }

    private void Start()
    {
        var canvas = Instantiate(m_canvas);
        canvas.transform.parent = this.transform.parent;
        for(int i = 0; i < canvas.transform.childCount; i++)
        {
            var child = canvas.transform.GetChild(i);
            var ui = child.GetComponent<DoorLockUI>();
            if(ui)
            {
                m_numberText = child.gameObject;
            }
        }
        m_digit = m_numberText.transform.childCount;
    }

    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.B))
        {
            AccessKey();
        }
    }

    /// <summary>
    /// �J���O�̏���
    /// </summary>
    public void AccessKey()
    {
        DecisionDoorNumber();
        m_numberText.GetComponent<DoorLockUI>().SetActiveUI(true);
        m_numberText.GetComponent<DoorLockUI>().ClearText();
        StartCoroutine("Unlock");
    }

    /// <summary>
    /// �A�N�Z�X���f
    /// </summary>
    public void Interruption()
    {
        Debug.Log("Access Interruption");
        m_numberText.GetComponent<DoorLockUI>().SetActiveUI(false);
        StopCoroutine("Unlock");
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
            m_correct = 0;
            m_almost = 0;
            numbers.Clear(); // �ԍ������Z�b�g
            InputPass(ref numbers); // �ԍ�����
            StartCoroutine("Collation", numbers); // �ƍ�

            yield break;
        }

        Debug.Log("unlocked");

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

        for (int i = 0; i < numbers.Count; i++)
        {
            if (numbers[i] == m_lockNumbers[i])
            {
                m_correct += 1;
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
                    m_almost++;
                    break;
                }
                yield return new WaitForEndOfFrame();
            }
        }

        if (m_correct == m_digit) // ��v�������������ƈꏏ�Ȃ�
        {
            m_isLock = false;
            StartCoroutine("Unlock");
            yield break;
        }

        m_numberText.GetComponent<DoorLockUI>().DisplayResult(m_correct, m_almost);

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
        var numbergene = m_generator.GetComponent<NumberLockGenerator>();
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
                }
            }
        }

        return result;
    }
}