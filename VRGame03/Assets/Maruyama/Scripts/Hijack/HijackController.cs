using System.Collections;
using System.Collections.Generic;
using UnityEngine;


/// <summary>
/// �����萧��
/// </summary>
public class HijackController : MonoBehaviour
{
    [System.Serializable]
    public class ParentParingData
    {
        private GameObject parentObject;
        public GameObject ParentObject { get => parentObject; set => parentObject = value; }
        public GameObject selfObject;
    }

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
    private List<ParentParingData> m_parentParingDatas = new List<ParentParingData>();

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

    [SerializeField]
    private GameObject DebugHiJackGameObject;

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
        if (PlayerInputer.IsDebugKeyDown(KeyCode.K))
        {
            StartHijack(DebugHiJackGameObject);
        }

        if (IsJack)
        {
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

        foreach(var data in m_parentParingDatas)
        {
            if(data.ParentObject == null) {
                continue;
            }

            data.selfObject.transform.parent = data.ParentObject.transform;
        }

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
        
        foreach(var data in m_parentParingDatas)
        {
            data.ParentObject = data.selfObject.transform.parent.gameObject;
            data.selfObject.transform.parent = null;
        }
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
