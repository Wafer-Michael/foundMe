using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface I_FactionMember
{
    /// <summary>
    /// �A�T�C������t�@�N�V������ݒ肷��B
    /// </summary>
    /// <param name="faction"></param>
    public void SetAssignedFaction(FactionCoordinator faction);

    /// <summary>
    /// �A�T�C�����Ă���t�@�N�V�������擾����B
    /// </summary>
    /// <returns></returns>
    public FactionCoordinator GetAssignedFaction();

    /// <summary>
    /// �t�@�N�V�����ɃA�T�C�������Ƃ��ɌĂяo����������
    /// </summary>
    /// <param name="faction"></param>
    public virtual void AssignedFactionEvent(FactionCoordinator faction) { }

    /// <summary>
    /// �t�@�N�V�������痣�E�����Ƃ��ɌĂяo����������
    /// </summary>
    /// <param name="faction"></param>
    public virtual void UnsignedFactionEvent(FactionCoordinator faction) { }

    /// <summary>
    /// �A�T�C������R�[�f�B�l�[�^��ݒ肷��B
    /// </summary>
    /// <param name="coordinator"></param>
    public void SetAssignedCoordinator(CoordinatorBase coordinator);

    /// <summary>
    /// �A�T�C�����Ă���R�[�f�B�l�[�^��ݒ肷��B
    /// </summary>
    /// <returns></returns>
    public CoordinatorBase GetAssignedCoordinator();

    /// <summary>
    /// �����o�[�ɉ��������Ƃ��ɌĂяo�������C�x���g
    /// </summary>
    public virtual void AssignCoordinatorEvent(CoordinatorBase coordinator) { }

    /// <summary>
    /// �����o�[���痣�E����Ƃ��ɌĂяo�������C�x���g
    /// </summary>
    public virtual void UnsignCoordinatorEvent(CoordinatorBase coordinator) { }
}
