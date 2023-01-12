using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UniRx;

public class Observer_JackUIPoint : MonoBehaviour
{
    [SerializeField]
    private Factory_Touch_JackUI m_factory;             //UI�����N���X

    [SerializeField]
    private JackCameraUI m_cameraUI;                    //�W���b�N���\������UI

    [SerializeField]
    private HijackController m_jackController;          //�W���b�N�R���g���[���[

    private Selectable_VRUI m_currentPointUI = null;    //���ݑI�𒆂�UI

    private void Start()
    {
        var pairDatas = m_factory.GetPariDatas();

        foreach (var data in pairDatas)
        {
            var ui = data.ui;
            var jakable = data.jakable;

            //�؂�ւ�����Ƃ��ɌĂт��������̓o�^
            ui.ObservableIsSelect
                .Skip(1)
                .Subscribe(value => jakable.UISelectEvent(value))
                .AddTo(this);

            ui.AddSelectEvent(TouchEvent);  //�^�b�`�������ɌĂяo����������
        }
    }

    private void TouchEvent(Selectable_VRUI pointUI)
    {
        if(m_currentPointUI == null) {
            m_currentPointUI = pointUI;
            m_cameraUI.FadeStart(FadeObject.FadeType.FadeOut);
            return;
        }

        //�����Ȃ�current��null�ɕς��ď������I��
        if(m_currentPointUI == pointUI) {
            Close();
            return;
        }

        m_currentPointUI.SetIsSelect(false);
        m_currentPointUI = pointUI; //���ݑI�𒆂�UI��ݒ�
    }

    /// <summary>
    /// �eUI���������Ƀ��Z�b�g����
    /// </summary>
    public void Close()
    {
        if (IsClose()) {    //���Ă���Ȃ珈�������Ȃ�
            return;
        }

        m_currentPointUI.SetIsSelect(false);
        m_currentPointUI = null;
        m_cameraUI.FadeStart(FadeObject.FadeType.FadeIn);
    }

    public bool IsClose() { return m_currentPointUI == null; }

    public void StartJack()
    {
        //�J�����gUI��null�Ȃ珈�����΂��B
        if(m_currentPointUI == null) {
            return;
        }

        m_jackController.StartHijack(FindSomeJakable(m_currentPointUI).gameObject);   //�W���b�N�J�n
    }

    private Jackable FindSomeJakable(Selectable_VRUI ui)
    {
        foreach(var data in m_factory.GetPariDatas())
        {
            if(data.ui == ui) {
                return data.jakable;
            }
        }

        return null;
    }
}
