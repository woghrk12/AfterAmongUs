using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum BulletType { FIVEMM, SEVENMM, TWELVEGAUGE }
public enum WeaponType { RIFLE, SHOTGUN }

public class PlayerWeapon : MonoBehaviour
{
    private Animator anim;
    private MainCamera mainCamera;

    [SerializeField] private Transform firePosition;
    [SerializeField] private GameObject bulletPrefab;
    [SerializeField] private GameObject muzzleFlash;
    public float rate;
    [SerializeField] private float recoil;
    private float cameraRecoil;
    [SerializeField] private WeaponType weaponType;
    public BulletType bulletType;
    public int maxAmmo;
    public int curAmmo;

    private void Awake()
    {
        anim = GetComponent<Animator>();
        mainCamera = Camera.main.GetComponent<MainCamera>();

        cameraRecoil = recoil * 0.2f;
    }
    
    public void Shot()
    {
        if (curAmmo <= 0) return;
        curAmmo--;

        StartCoroutine(ShotFlash());

        var t_recoil = weaponType != WeaponType.SHOTGUN ? Random.Range(-recoil, recoil) : 0f;
        firePosition.Rotate(0f, 0f, t_recoil);
        Instantiate(bulletPrefab, firePosition.position, firePosition.rotation);
        firePosition.Rotate(0f, 0f, -t_recoil);

        anim.SetTrigger("Shot");
        mainCamera.SetCameraShake(cameraRecoil, 0.1f);
    }

    private IEnumerator ShotFlash()
    {
        muzzleFlash.SetActive(true);
        yield return new WaitForSeconds(0.05f);
        muzzleFlash.SetActive(false);
    }

    public int Reload(int playerAmmo)
    {
        var reloadAmmo = playerAmmo < maxAmmo ? playerAmmo : maxAmmo;
        curAmmo = maxAmmo;
        return reloadAmmo;
    }
}

