using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerWeapon : MonoBehaviour
{
    private Animator anim;

    [SerializeField] private Transform triggerPosition;
    [SerializeField] private Transform firePosition;
    private Vector3 direction;
    private float angle;

    public float range;
    public float rate;
    [SerializeField] private float recoil;
    private float cameraRecoil;
    public float reloadTime;

    [SerializeField] private EWeaponType weaponType;
    
    public EBulletType bulletType;
    private string bulletTag;
    
    public int maxAmmo;
    public int curAmmo;

    private void Awake()
    {
        anim = GetComponent<Animator>();

        cameraRecoil = recoil * 0.2f;

        switch (bulletType)
        {
            case EBulletType.FIVEMM:
                bulletTag = "5mm Bullet";
                break;
            case EBulletType.SEVENMM:
                bulletTag = "5mm Bullet";
                break;
            case EBulletType.TWELVEGAUGE:
                bulletTag = "12 Gauge Bullet";
                break;
            case EBulletType.NINEMM:
                bulletTag = "9mm Bullet";
                break;
        }

        curAmmo = maxAmmo;
    }
    
    public void Shot()
    {
        if (curAmmo <= 0) return;
        curAmmo--;

        direction = (firePosition.position - triggerPosition.position).normalized;
        angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        switch (weaponType)
        {
            case EWeaponType.PISTOL:
            case EWeaponType.RIFLE:
                ObjectPooling.SpawnObject("Single Muzzle Flash", firePosition.position, Quaternion.AngleAxis(angle, Vector3.forward));
                SingleShot(direction);
                break;

            case EWeaponType.SHOTGUN:
                ObjectPooling.SpawnObject("Multiple Muzzle Flash", firePosition.position, Quaternion.AngleAxis(angle, Vector3.forward));
                MultiShot(direction);
                break;
        }

        anim.SetTrigger("Shot");
        CameraShaking.SetCameraShake(cameraRecoil, 0.1f);
    }

    private void SingleShot(Vector3 p_dir)
    {
        var t_recoil = Random.Range(-recoil, recoil);
        var t_bullet = ObjectPooling.SpawnObject(bulletTag, firePosition.position, firePosition.rotation).GetComponent<Bullet>();
        var t_dir = Quaternion.AngleAxis(t_recoil, Vector3.forward) * p_dir;
        t_bullet.SetDirection(t_dir);
    }

    private void MultiShot(Vector3 p_dir)
    {
        for (int i = 0; i < 5; i++)
        {
            var t_recoil = Random.Range(-recoil, recoil) * 0.5f;
            var t_bullet = ObjectPooling.SpawnObject(bulletTag, firePosition.position, firePosition.rotation).GetComponent<Bullet>();
            var t_dir = Quaternion.AngleAxis(t_recoil, Vector3.forward) * p_dir;
            t_bullet.SetDirection(t_dir);
        }
    }

    public int Reload(int p_playerAmmo)
    {
        var t_needAmmo = maxAmmo - curAmmo;
        var t_reloadAmmo = p_playerAmmo < t_needAmmo ? p_playerAmmo : t_needAmmo;
        curAmmo += t_reloadAmmo;
        return t_reloadAmmo;
    }
}

