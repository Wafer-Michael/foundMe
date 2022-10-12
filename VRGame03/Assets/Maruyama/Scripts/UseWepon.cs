using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UseWepon : MonoBehaviour
{

    [SerializeField]
    private WeaponBase m_weapon;            //所持している武器
    public WeaponBase Weapon => m_weapon;   //所持している武器のプロパティ

    private void Update()
    {
        if (m_weapon == null)
        {
            Debug.Log("UseWeapon::武器がnullです。");
            return;
        }

        if (PlayerInputer.IsShot())
        {
            Weapon.Shot(transform.forward);
        }
    }
}
