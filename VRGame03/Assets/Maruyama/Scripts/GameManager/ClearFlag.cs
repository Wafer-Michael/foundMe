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

        //ジャック中なら判定をしない
        if (m_jackController.IsJack) {
            Debug.Log("NowHijack");
            return;
        }

        //playerでないなら反応しない。
        var player = other.GetComponentInParent<PCPlayer>();
        if (!player) {
            Debug.Log("NotPlayer");
            return;
        }

        GameManagerComponent.Instance.ChangeState(GameManagerComponent.GameState.Clear);
    }
}
