using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JackPointUI : MonoBehaviour
{
    private Jackable m_jakable = null;  //�����̐ݒ肳��Ă���n�b�L���O����镨

    //--------------------------------------------------------------------------------------
    /// �A�N�Z�b�T
    //--------------------------------------------------------------------------------------

    public void SetJakable(Jackable jakable) { m_jakable = jakable; }

    public Jackable GetJakable() { return m_jakable; }

}
