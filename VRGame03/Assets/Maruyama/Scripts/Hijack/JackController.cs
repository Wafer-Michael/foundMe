using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JackController : MonoBehaviour
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
    Parametor m_param;  //�p�����[�^
    public Parametor Param
    {
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
    public bool IsJack
    {
        private set => m_isJack.Value = value;
        get => m_isJack.Value;
    }

    [SerializeField]
    private DissolveFadeSprite m_dissolveFadeSprite;    //�f�B�]�u���V�F�[�_�[

    [SerializeField]
    private GameObject m_returnTouchUI;                 //�߂�UI���菈��

    private void Awake() {
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

    public void ForceCamBack()
    {
        CamBack();
    }

    /// <summary>
    /// �n�C�W���b�N����߂Č��ɖ߂�B
    /// </summary>
    private void CamBack()
    {
        if (!IsJack) {  //�W���b�N��ԂłȂ��Ȃ�A���K�v���Ȃ��B
            return;
        }

        //�t�F�[�h�J�n
        UnityEngine.Events.UnityAction finishAction = () => m_dissolveFadeSprite.FadeStart(FadeObject.FadeType.FadeIn);
        finishAction += () => {
            transform.position = m_camBackData.position;
            transform.forward = m_camBackData.forward;
            IsJack = false;
        };

        m_dissolveFadeSprite.FadeStart(FadeObject.FadeType.FadeOut, finishAction);  //�t�F�[�h�X�^�[�g
        m_returnTouchUI.SetActive(false);
    }

    /// <summary>
    /// �n�C�W���b�N�J�n
    /// </summary>
    /// <param name="target">�n�C�W���b�N�^�[�Q�b�g</param>
    public void StartHijack(Jackable target)
    {
        //Jack���Ȃ珈�����΂��B
        if (IsJack) {
            return;
        }

        if (target == null) {   //�^�[�Q�b�g��nullCheck
            return;
        }

        SaveCamBackData();      //�߂�ꏊ���L�^����B

        Jack(target);

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

    private void Jack(Jackable target)
    {
        transform.position = target.transform.position + target.PositionOffset;
        transform.forward = target.transform.forward;

        m_returnTouchUI.SetActive(true);
    }

    /// <summary>
    /// �C���^�[�o���^�C�����[�g�̎擾
    /// </summary>
    /// <returns></returns>
    public float GetIntervalTimeRate() => m_timer.IntervalTimeRate;
}
