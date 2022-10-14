using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shotgun : Weapon
{
    [SerializeField] private MultiFire fireController = null;

    protected override void Shot(Vector3 p_firePos, Vector3 p_dir)
    {
        var t_muzzleFlash = ObjectPoolingManager.SpawnObject(MuzzleFlash).GetComponent<MuzzleFlash>();
        t_muzzleFlash.SetFlash(p_firePos, p_dir);
        t_muzzleFlash.ShowFlash();

        fireController.Fire(Bullet, p_firePos, p_dir, Accurate);
    }
}
