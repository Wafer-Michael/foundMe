using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface I_Decorator
{

	/// <summary>
	/// �������Ɉ�x�����Ăԏ���
	/// </summary>
	void OnCreate();

	/// <summary>
	/// �m�[�h�J�n���ɌĂяo�������B
	/// </summary>
	void OnStart();

	/// <summary>
	/// �m�[�h�I�����ɌĂяo������
	/// </summary>
	void OnExit();

	/// <summary>
	/// �J�ڏ����m�F�O�̏���(CanTransition���ĂԑO�ɌĂԏ���)
	/// </summary>
	void ReserveCanTransition();

	/// <summary>
	/// �J�ڂł��邩�ǂ���
	/// </summary>
	/// <returns>�J�ڂł���Ȃ�true</returns>
	bool CanTransition();

	/// <summary>
	/// �A�b�v�f�[�g���\���ǂ���
	/// </summary>
	/// <returns>�A�b�v�f�[�g���\�Ȃ�true</returns>
	bool CanUpdate();
}
