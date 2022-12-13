using System.Collections;
using System.Collections.Generic;
using UnityEngine;


/// <summary>
/// �����萧��
/// </summary>
public class HijackController : MonoBehaviour
{
    /// <summary>
    /// �p�����[�^
    /// </summary>
    [System.Serializable]
    public struct Parametor
    {
        public float time; //����
    }


    /// <summary>
    /// �߂�p�̃f�[�^
    /// </summary>
    public struct CamBackData
    {
        public Vector3 position;
        public Vector3 forward;
    }

    [SerializeField]
    private GameObject m_modelObject;       //���f���̃I�u�W�F�N�g

    [SerializeField]
    private GameObject m_modelParentObject; //���f���̐e�I�u�W�F�N�g

    [SerializeField]
    Parametor m_param;  //�p�����[�^
    public Parametor Param {
        get => m_param;
        set => m_param = value;
    }

    CamBackData m_camBackData;  //�߂�f�[�^
    public CamBackData CamBackDataProperty
    {
        get => m_camBackData;
        set => m_camBackData = value;
    }

    private GameTimer m_timer;      //�^�C�}�[

    //�W���b�N����\���B
    private UniRx.ReactiveProperty<bool> m_isJack = new UniRx.ReactiveProperty<bool>(false);
    public System.IObservable<bool> IsJackObserver => m_isJack;
    public bool IsJack {
        private set => m_isJack.Value = value; 
        get => m_isJack.Value;
    }

    private void Awake()
    {
        m_timer = new GameTimer(0.0f);    
    }

    private void Start()
    {
        m_timer.ResetTimer(m_param.time);
        m_camBackData.position = transform.position;
    }

    private void Update()
    {
        if (IsJack)
        {
            //�߂�{�^������������A
            if (true)
            {
                //CamBack()����B
                //���̌�̏������Ȃ�
            }

            m_timer.UpdateTimer();

            if (m_timer.IsTimeUp)   //�^�C���A�b�v������ATimeOver����
            {
                TimeOver();
            }
        }
    }

    /// <summary>
    /// ���Ԑ؂�̎��̏���
    /// </summary>
    private void TimeOver()
    {
        CamBack();
    }

    /// <summary>
    /// �n�C�W���b�N����߂Č��ɖ߂�B
    /// </summary>
    private void CamBack()
    {
        transform.position = m_camBackData.position;
        transform.forward = m_camBackData.forward;

        m_modelObject.transform.parent = m_modelParentObject.transform;    //���f���̐e��߂��B
        IsJack = false;
    }

    /// <summary>
    /// �n�C�W���b�N�J�n
    /// </summary>
    /// <param name="target">�n�C�W���b�N�^�[�Q�b�g</param>
    public void StartHijack(GameObject target)
    {
        //Jack���Ȃ珈�����΂��B
        if (IsJack) {   
            return;
        }

        SaveCamBackData();
        m_modelObject.transform.parent = null;  //���f���̐e�q�֌W���O���B
        Warp(target);

        m_timer.ResetTimer(m_param.time);       //�^�C�}�[�X�^�[�g
        IsJack = true;
    }

    /// <summary>
    /// �߂�p�̃f�[�^���擾
    /// </summary>
    private void SaveCamBackData()
    {
        m_camBackData.position = transform.position;
        m_camBackData.forward = transform.forward;
    }

    private void Warp(GameObject target)
    {
        transform.position = target.transform.position;
        transform.forward = target.transform.forward;
    }
}
