using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClearFlag : MonoBehaviour
{
    [SerializeField]
    HijackController m_jackController;

    private void OnTriggerEnter(Collider other)
    {
        //ジャック中なら判定をしない
        if (m_jackController.IsJack) {
            return;
        }

        //playerでないなら反応しない。
        var player = other.GetComponent<PCPlayer>();
        if (!player) {
            return;
        }

        GameManagerComponent.Instance.ChangeState(GameManagerComponent.GameState.Clear);
    }
}
