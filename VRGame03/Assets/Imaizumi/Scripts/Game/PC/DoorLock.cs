using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// �h�A�̈Ïؔԍ��ƌ��̏�Ԃ��Ǘ�����
/// </summary>
public class DoorLock : MonoBehaviour
{
    int m_digit = 3; // �Ïؔԍ��̌���

    List<int> m_lockNumbers = new List<int>(); // �Ïؔԍ�

    [SerializeField]
    bool m_isLock = true; // �����������Ă��邩�ǂ���
    public bool IsLock { get { return m_isLock; }}

    [SerializeField]
    GameObject m_canvas; // NumberText�̐e�I�u�W�F�N�g
    GameObject m_numberText; // ��������NumberText

    GameObject m_generator; // �ԍ������@

    //int[] m_collationNumbers = new int[3];

    int m_correct = 0;    //��v
    int m_almost = 0;     //�ɂ���

    [SerializeField]
    int m_maxNumError; // ���s�ł���ő吔
    int m_numError; // ���s������

    System.Action m_action; // �J�����̃C�x���g
    System.Action m_errEvent; // �G���[���̃C�x���g

    [SerializeField]
    AudioSource m_unlockSE; // �J������SE
    [SerializeField]
    AudioSource m_errSE; // �G���[����SE

    [SerializeField]
    GameObject m_front;
    [SerializeField]
    GameObject m_back;

    private void Awake()
    {
        m_generator = GameObject.Find("NumberLockGenerator");
    }

    private void Start()
    {
        var canvas = Instantiate(m_canvas); // �L�����o�X����
        canvas.transform.parent = this.transform.parent; // �L�����o�X�̐e��Room�ɐݒ�

        // NumberText���擾    
        for(int i = 0; i < canvas.transform.childCount; i++)
        {
            var child = canvas.transform.GetChild(i);
            var ui = child.GetComponent<DoorLockUI>();
            if(ui)
            {
                m_numberText = child.gameObject;
            }
        }

        m_digit = m_numberText.transform.childCount; // �������X�V
    }

    /// <summary>
    /// �J���O�̏���
    /// </summary>
    public void AccessKey(GameObject other)
    {
        if (!m_numberText.gameObject.activeInHierarchy)
        {
            DecisionDoorNumber();
            //�h�A�̏ꏊ��ݒ�B
            float convart = ConvartDirection(other);
            
            //m_numberText.transform.parent.transform.position = transform.position + new Vector3(-0.625f, -2.75f, 0.15f);
            if(convart == 1) {
                m_numberText.transform.parent.transform.position = m_front.transform.position;
                m_numberText.transform.parent.transform.rotation = m_front.transform.rotation;
            }
            else {
                m_numberText.transform.parent.transform.position = m_back.transform.position;
                m_numberText.transform.parent.transform.rotation = m_back.transform.rotation;
            }
            
            m_numberText.GetComponent<DoorLockUI>().SetActiveUI(true);
            m_numberText.GetComponent<DoorLockUI>().ClearText();
            StartCoroutine("Unlock");
        }
    }

    private int ConvartDirection(GameObject other)
    {
        var requesterToOwner = transform.position - other.transform.position;
        float newDot = Vector3.Dot(requesterToOwner, transform.forward);

        return newDot > 0 ? -1 : 1;
    }



    /// <summary>
    /// �A�N�Z�X���f
    /// </summary>
    public void Interruption()
    {
        m_numberText.GetComponent<DoorLockUI>().SetActiveUI(false);
        StopCoroutine("Unlock");
    }

    /// <summary>
    /// �����J����
    /// </summary>
    IEnumerator Unlock()
    {
        List<int> numbers = new List<int>();

        while (m_isLock) // �J������ĂȂ�������
        {
            yield return new WaitWhile(() => !PlayerInputer.IsEnter()); // �X�y�[�X���������܂őҋ@
            // �e���ڂ����Z�b�g
            m_correct = 0;
            m_almost = 0;
            numbers.Clear();

            InputPass(ref numbers); // �ԍ�����
            StartCoroutine("Collation", numbers); // �ƍ�

            yield break;
        }

        // �J�����̏���
        m_action?.Invoke(); // �C�x���g�Ăяo��
        m_unlockSE.PlayOneShot(m_unlockSE.clip);
        Interruption(); // �A�N�Z�X���f

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
            var numberText = m_numberText.transform.GetChild(i).gameObject; // NumberText���擾

            int textNum = int.Parse(numberText.GetComponent<Text>().text);
            numbers.Add(textNum); // ������ǉ�
        }
    }

    /// <summary>
    /// ���͂����ԍ��������������肷��
    /// </summary>
    /// <param name="numbers">���͂���ԍ�</param>
    IEnumerator Collation(List<int> numbers)
    {
        // �������𔻒�
        for (int i = 0; i < numbers.Count; i++)
        {
            if (numbers[i] == m_lockNumbers[i])
            {
                m_correct += 1;
                numbers[i]  = -1;
            }
            yield return new WaitForEndOfFrame();
        }

        // �ɂ����������𔻒�
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
            m_isLock = false; // �J��
            StartCoroutine("Unlock");
            yield break;
        }

        // �ȉ��J�����s��

        m_numberText.GetComponent<DoorLockUI>().DisplayResult(m_correct, m_almost); // �t�B�[�h�o�b�N�\��
        m_numError++; // �G���[�񐔍X�V
        m_errSE.PlayOneShot(m_errSE.clip);
        if(m_numError >= m_maxNumError) // �G���[���ő吔�ɒB������
        {
            m_errEvent?.Invoke(); // �C�x���g�Ăяo��
        }

        StartCoroutine("Unlock");
        yield break;
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
    /// �Ïؔԍ���ݒ肷��
    /// </summary>
    /// <param name="numbers">�Ïؔԍ�</param>
    public void SetLockNumbers(List<int> numbers)
    {
        foreach (int num in numbers)
        {
            m_lockNumbers.Add(num);
        }
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

        var count = shader.GetPropertyCount();
        for (int i = 0; i < count; i++)
        {
            var type = shader.GetPropertyType(i);
            if (type ==UnityEngine.Rendering.ShaderPropertyType.Texture)
            {
                var proName = shader.GetPropertyName(i);
                var tex = mat.GetTexture(proName);
                if (tex)
                {
                    result = tex;
                }
            }
        }

        return result;
    }

    /// <summary>
    /// �J�����̃C�x���g�ݒ�
    /// </summary>
    /// <param name="action">�C�x���g</param>
    public void SetAction(System.Action action)
    {
        m_action = action;
    }

    /// <summary>
    /// �G���[�������̃C�x���g�ݒ�
    /// </summary>
    /// <param name="action">�C�x���g</param>
    public void SetErrorEvent(System.Action action)
    {
        m_errEvent = action;
    }
}