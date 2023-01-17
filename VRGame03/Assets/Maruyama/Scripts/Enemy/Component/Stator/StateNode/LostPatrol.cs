using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// player�������������̜p�j�X�e�[�g
/// </summary>
public class LostPatrol : EnemyStateNodeBase<EnemyBase>
{
    public struct Parametor 
    {
        public float time;      //�T��������ő厞��
    }

    private Parametor m_param;  //�p�����[�^

    private GameTimer m_timer;  //�^�C�}�[�Ǘ��N���X

    public LostPatrol(EnemyBase owner) :
        base(owner)
    { }

    public LostPatrol(EnemyBase owner, Parametor parametor) :
        base(owner)
    {
        m_param = parametor;
        m_timer = new GameTimer();
    }

    public override void OnStart()
    {
        base.OnStart();

        //�e���}�b�v�����ɁA�p�j���[�g�����߂�B
        
    }

    public override bool OnUpdate()
    {
        //��ɉe���}�b�v���X�V��������B

        
        m_timer.UpdateTimer();   //���Ԍv��

        return IsEnd();          //��莞�Ԃ�������A�T�����I������B
    }

    private bool IsEnd() { return m_timer.IsTimeUp; }
}
