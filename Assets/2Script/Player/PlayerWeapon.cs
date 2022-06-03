using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum BulletType { FIVEMM, SEVENMM, TWELVEGAUGE }
public enum WeaponType { RIFLE, SHOTGUN }

public class PlayerWeapon : MonoBehaviour
{
    private Animator anim;

    [SerializeField] private Transform triggerPosition;
    [SerializeField] private Transform firePosition;
    private Vector3 direction;

    [SerializeField] private GameObject muzzleFlash;
    
    public float rate;
    [SerializeField] private float recoil;
    private float cameraRecoil;
    public float reloadTime;

    [SerializeField] private WeaponType weaponType;
    
    public BulletType bulletType;
    private string bulletTag;
    
    public int maxAmmo;
    public int curAmmo;

    private void Awake()
    {
        anim = GetComponent<Animator>();

        cameraRecoil = recoil * 0.2f;

        switch (bulletType)
        {
            case BulletType.FIVEMM:
                bulletTag = "5mm Bullet";
                break;
            case BulletType.SEVENMM:
                bulletTag = "5mm Bullet";
                break;
            case BulletType.TWELVEGAUGE:
                bulletTag = "12 Gauge Bullet";
                break;
        }
    }
    
    public void Shot()
    {
        if (curAmmo <= 0) return;
        curAmmo--;

        StartCoroutine(ShotFlash());

        direction = (firePosition.position - triggerPosition.position).normalized;

        switch (weaponType)
        {
            case WeaponType.RIFLE:
                UseRifle(direction);
                break;

            case WeaponType.SHOTGUN:
                UseShotgun(direction);
                break;
        }

        anim.SetTrigger("Shot");
        CameraShaking.SetCameraShake(cameraRecoil, 0.1f);
    }

    private void UseRifle(Vector3 p_dir)
    {
        var t_recoil = Random.Range(-recoil, recoil);
        var t_bullet = ObjectPooling.SpawnObject(bulletTag, firePosition.position, firePosition.rotation).GetComponent<Bullet>();
        var t_dir = Quaternion.AngleAxis(t_recoil, Vector3.forward) * p_dir;
        t_bullet.SetDirection(t_dir);
    }

    private void UseShotgun(Vector3 p_dir)
    {
        for (int i = 0; i < 5; i++)
        {
            var t_recoil = Random.Range(-recoil, recoil) * 0.5f;
            var t_bullet = ObjectPooling.SpawnObject(bulletTag, firePosition.position, firePosition.rotation).GetComponent<Bullet>();
            var t_dir = Quaternion.AngleAxis(t_recoil, Vector3.forward) * p_dir;
            t_bullet.SetDirection(t_dir);
        }
    }

    private IEnumerator ShotFlash()
    {
        muzzleFlash.SetActive(true);
        yield return new WaitForSeconds(0.05f);
        muzzleFlash.SetActive(false);
    }

    public int Reload(int p_playerAmmo)
    {
        var t_needAmmo = maxAmmo - curAmmo;
        var t_reloadAmmo = p_playerAmmo < t_needAmmo ? p_playerAmmo : t_needAmmo;
        curAmmo += t_reloadAmmo;
        return t_reloadAmmo;
    }
}

