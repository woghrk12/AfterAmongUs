using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerWeapon : MonoBehaviour
{
    [SerializeField] private Weapon[] weapons = null;
    private Weapon[] hasWeapon = new Weapon[2];
    private Weapon equipWeapon = null;
    public Weapon EquipWeapon { get { return equipWeapon; } }

    public void InitWeapon()
    {
        var t_weapons = GameManager.playerWeapon;

        for (int i = 0; i < t_weapons.Length; i++)
            hasWeapon[i] = weapons[(int)t_weapons[i]];

        ChangeWeapon(0);
    }

    public void ChangeWeapon(int p_idx)
    {
        equipWeapon = hasWeapon[p_idx];
        for(int i = 0; i < hasWeapon.Length; i++)
            hasWeapon[i].gameObject.SetActive(p_idx == i);
    }

    public void UseWeapon() => equipWeapon.UseWeapon();
}
