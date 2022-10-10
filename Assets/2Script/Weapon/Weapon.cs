using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon : MonoBehaviour
{
    [SerializeField] private Animator anim = null;

    [SerializeField] private Transform triggerPosition = null;
    [SerializeField] private Transform firePosition = null;
    [SerializeField] private float range = 0f;
    [SerializeField, Range(0f, 1f)] private float recoilPower = 0f;
    [SerializeField, Range(0f, 5f)] private float accurate = 0f;
    [SerializeField] private float fireRate = 0f;

    public float Range { get { return range; } }
    public float FireRate { get { return fireRate; } }

    [SerializeField] private string bullet = null;
    [SerializeField] private string muzzleFlash = null;

    private void Awake()
    {
        anim.SetFloat("motionSpeed", 1.5f - recoilPower);
    }

    public void UseWeapon() => Shot(firePosition.position, firePosition.position - triggerPosition.position);

    private void Shot(Vector3 p_firePos, Vector3 p_dir)
    {
        var t_muzzleFlash = ObjectPoolingManager.SpawnObject(muzzleFlash).GetComponent<MuzzleFlash>();
        t_muzzleFlash.SetFlash(p_firePos, p_dir);
        t_muzzleFlash.ShowFlash();

        var t_bullet = ObjectPoolingManager.SpawnObject(bullet).GetComponent<Bullet>();
        var t_dir = Quaternion.AngleAxis(Random.Range(-accurate, accurate), Vector3.forward) * p_dir;
        t_bullet.InitValue(p_firePos, t_dir);
        anim.SetTrigger("Shot");
    }
}
