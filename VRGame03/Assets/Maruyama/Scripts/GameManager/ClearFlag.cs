using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClearFlag : MonoBehaviour
{
    [SerializeField]
    HijackController m_jackController;

    private void OnTriggerEnter(Collider other)
    {
        Debug.Log("TriggerEnter");

        //�W���b�N���Ȃ画������Ȃ�
        if (m_jackController.IsJack) {
            Debug.Log("NowHijack");
            return;
        }

        //player�łȂ��Ȃ甽�����Ȃ��B
        var player = other.GetComponentInParent<PCPlayer>();
        if (!player) {
            Debug.Log("NotPlayer");
            return;
        }

        GameManagerComponent.Instance.ChangeState(GameManagerComponent.GameState.Clear);
    }
}
