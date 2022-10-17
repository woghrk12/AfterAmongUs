using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerWeapon : MonoBehaviour
{
    [SerializeField] private ControlStatus bulletStatus = null;
    [SerializeField] private Weapon[] weapons = null;
    private Weapon[] hasWeapon = new Weapon[2];
    private Weapon equipWeapon = null;

    public Weapon EquipWeapon { get { return equipWeapon; } }

    private float fireDelay = 0f;

    private void Update()
    {
        fireDelay += Time.deltaTime;
    }

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
        fireDelay = equipWeapon.FireRate;

        for(int i = 0; i < hasWeapon.Length; i++)
            hasWeapon[i].gameObject.SetActive(p_idx == i);

        bulletStatus.MaxValue = equipWeapon.MaxBullet;
        bulletStatus.SetValue(equipWeapon.CurBullet);
    }

    public void UseWeapon()
    {
        if (fireDelay < equipWeapon.FireRate) return;
        if (!equipWeapon.CheckCanShot()) return;

        fireDelay = 0f;
        equipWeapon.UseWeapon();
        bulletStatus.SetValue(equipWeapon.CurBullet);
    }
}
