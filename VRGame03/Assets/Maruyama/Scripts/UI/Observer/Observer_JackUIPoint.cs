using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UniRx;

public class Observer_JackUIPoint : MonoBehaviour
{
    public struct Data
    {
        public Jackable jakable;
        public JackPointUI ui;

        public Data(Jackable jackable, JackPointUI ui)
        {
            this.jakable = jackable;
            this.ui = ui;
        }
    }

    private List<Data> m_datas = new List<Data>();

    private List<JackPointUI> m_observeUIs = new List<JackPointUI>();

    [SerializeField]
    private Factory_Touch_JackUI m_factory;

    [SerializeField]
    private JackCameraUI m_cameraUI;               //�W���b�N���\������UI

    private JackPointUI m_currentPointUI = null;   //���ݑI�𒆂�UI

    private void Start()
    {
        m_observeUIs = m_factory.GetJackPointUIs();

        foreach (var ui in m_observeUIs)
        {
            m_datas.Add(new Data(ui.GetJakable(), ui));

            //�؂�ւ�����Ƃ��ɌĂт��������̓o�^
            ui.ObservableIsSelect
                .Skip(1)
                .Subscribe(value => ui.GetJakable().UISelectEvent(value))
                .AddTo(this);

            ui.AddSelectEvent(TouchEvent);
        }
    }

    private void TouchEvent(JackPointUI pointUI)
    {
        //�����Ȃ�current��null�ɕς��ď������I��
        if(m_currentPointUI == pointUI) {
            m_currentPointUI = null;
            return;
        }

        m_currentPointUI.SetIsSelect(false);
        m_currentPointUI = pointUI; //���ݑI�𒆂�UI��ݒ�
    }
}
