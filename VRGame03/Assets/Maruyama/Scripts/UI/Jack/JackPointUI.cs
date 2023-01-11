using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JackPointUI : MonoBehaviour
{
    private Jackable m_jakable = null;  //自分の設定されているハッキングされる物

    //--------------------------------------------------------------------------------------
    /// アクセッサ
    //--------------------------------------------------------------------------------------

    public void SetJakable(Jackable jakable) { m_jakable = jakable; }

    public Jackable GetJakable() { return m_jakable; }

}
