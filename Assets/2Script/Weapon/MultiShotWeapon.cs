using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MultiShotWeapon : Weapon
{
    protected override void Shot(Transform p_origin, Vector3 p_dir)
    {
        for (int i = 0; i < 5; i++)
        {
            var t_recoil = Random.Range(-recoil, recoil) * 0.5f;
            var t_bullet = ObjectPooling.SpawnObject(bulletTag, p_origin.position, p_origin.rotation).GetComponent<Bullet>();
            var t_dir = Quaternion.AngleAxis(t_recoil, Vector3.forward) * p_dir;
            t_bullet.SetDirection(t_dir);
        }
    }
}
