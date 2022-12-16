using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerWeapon : MonoBehaviour
{
    [SerializeField] private Weapon[] weapons = null;
    private Weapon[] hasWeapon = new Weapon[2];
    private Weapon equipWeapon = null;

    public Weapon EquipWeapon { get { return equipWeapon; } }

    private float fireDelay = 0f;

    private void Update()
    {
        fireDelay += Time.deltaTime;
    }

    public void InitWeapon(StatusUI p_statusUI = null)
    {
        var t_weapons = GameManager.playerWeapon;

        for (int i = 0; i < t_weapons.Length; i++)
        {
            hasWeapon[i] = weapons[(int)t_weapons[i]];
            ObjectPoolingManager.instance.SetFlag(hasWeapon[i].Bullet, true);
        }
        
        ChangeWeapon(0);

        if (!p_statusUI) return;
        p_statusUI.SetBulletStatus(equipWeapon);
    }

    public void ChangeWeapon(int p_idx, ControlSlider p_bulletStatus = null)
    {
        equipWeapon = hasWeapon[p_idx];
        fireDelay = equipWeapon.FireRate;

        for(int i = 0; i < hasWeapon.Length; i++)
            hasWeapon[i].gameObject.SetActive(p_idx == i);

        if (!p_bulletStatus) return;
        p_bulletStatus.SetMaxValue(equipWeapon.MaxBullet);
        p_bulletStatus.SetValue(equipWeapon.CurBullet);
    }

    public void UseWeapon(ControlSlider p_bulletStatus = null)
    {
        if (fireDelay < equipWeapon.FireRate) return;
        if (!equipWeapon.CheckCanShot()) return;

        fireDelay = 0f;
        equipWeapon.UseWeapon();

        if (!p_bulletStatus) return;
        p_bulletStatus.SetValue(equipWeapon.CurBullet);
    }

    public IEnumerator Reload(ControlSlider p_bulletStatus = null)
    {
        if (equipWeapon.CurBullet.Equals(equipWeapon.MaxBullet)) yield break;

        yield return Utilities.WaitForSeconds(equipWeapon.ReloadTime);
        equipWeapon.Reload();

        if (!p_bulletStatus) yield break;
        p_bulletStatus.SetValue(equipWeapon.CurBullet);
    }
}
