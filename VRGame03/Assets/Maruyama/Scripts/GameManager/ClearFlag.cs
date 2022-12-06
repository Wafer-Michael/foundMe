using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClearFlag : MonoBehaviour
{
    [SerializeField]
    HijackController m_jackController;

    private void OnTriggerEnter(Collider other)
    {
        //�W���b�N���Ȃ画������Ȃ�
        if (m_jackController.IsJack) {
            return;
        }

        //player�łȂ��Ȃ甽�����Ȃ��B
        var player = other.GetComponent<PCPlayer>();
        if (!player) {
            return;
        }

        GameManagerComponent.Instance.ChangeState(GameManagerComponent.GameState.Clear);
    }
}
