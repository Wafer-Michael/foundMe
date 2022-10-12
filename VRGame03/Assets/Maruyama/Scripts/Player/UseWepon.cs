using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UseWepon : MonoBehaviour
{

    [SerializeField]
    private WeaponBase m_weapon;            //�������Ă��镐��
    public WeaponBase Weapon => m_weapon;   //�������Ă��镐��̃v���p�e�B

    private void Update()
    {
        if (m_weapon == null)
        {
            Debug.Log("UseWeapon::���킪null�ł��B");
            return;
        }

        if (PlayerInputer.IsShot())
        {
            Weapon.Shot(transform.forward);
        }
    }
}
