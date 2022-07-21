using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Weapon : MonoBehaviour
{
    private Animator anim = null;

    [SerializeField] private Transform triggerPosition = null;
    [SerializeField] private Transform firePosition = null;
    private Vector3 direction = Vector3.zero;
    private float angle = 0f;

    public float range = 0f;
    public float rate = 0f;
    [SerializeField] protected float recoil = 0f;
    private float cameraRecoil = 0f;
    public float reloadTime = 0f;

    public EBulletType bulletType = EBulletType.END;
    protected string bulletTag = "";

    public int maxAmmo = 0;
    public int curAmmo = 0;

    private void Awake()
    {
        anim = GetComponent<Animator>();
    }

    private void Start()
    {
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

    protected abstract void Shot(Transform p_origin, Vector3 p_dir);

    public void UseWeapon()
    {
        if (curAmmo <= 0) return;
        curAmmo--;

        direction = (firePosition.position - triggerPosition.position).normalized;
        angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        ObjectPooling.SpawnObject("Muzzle Flash", firePosition.position, Quaternion.AngleAxis(angle, Vector3.forward));
        Shot(firePosition, direction);
        anim.SetTrigger("Shot");
        CameraShaking.SetCameraShake(cameraRecoil, 0.1f);
    }

    public int Reload(int p_playerAmmo)
    {
        var t_needAmmo = maxAmmo - curAmmo;
        var t_reloadAmmo = p_playerAmmo < t_needAmmo ? p_playerAmmo : t_needAmmo;
        curAmmo += t_reloadAmmo;
        return t_reloadAmmo;
    }
}
