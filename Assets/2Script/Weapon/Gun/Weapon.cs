using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Weapon : MonoBehaviour
{
    [SerializeField] private Animator anim = null;
    private CameraShaking cameraShake = null;

    [SerializeField] private Transform triggerPosition = null;
    [SerializeField] private Transform firePosition = null;
    [SerializeField] private float range = 0f;
    [SerializeField, Range(0f, 1f)] private float recoilPower = 0f;
    [SerializeField, Range(0f, 5f)] private float accurate = 0f;
    [SerializeField] private float fireRate = 0f;
    [SerializeField] private string bullet = null;
    [SerializeField] private string muzzleFlash = null;
    [SerializeField] private int maxBullet = 0;
    private int curBullet = 0;
    [SerializeField] private float reloadTime = 0f;

    public float Range { get { return range; } }
    public float FireRate { get { return fireRate; } }
    public int MaxBullet { get { return maxBullet; } }
    public int CurBullet { get { return curBullet; } }
    public float ReloadTime { get { return reloadTime; } }
    protected float Accurate { get { return accurate; } }
    public string Bullet { get { return bullet; } }
    protected string MuzzleFlash { get { return muzzleFlash; } }
    
    protected void Awake()
    {
        anim.SetFloat("motionSpeed", 1.5f - recoilPower);
        cameraShake = Camera.main.GetComponent<CameraShaking>();
        curBullet = maxBullet;
    }

    public void UseWeapon()
    {
        curBullet--;
        Shot(firePosition.position, firePosition.position - triggerPosition.position);
        cameraShake.ShakeCamera(recoilPower * 10f, recoilPower);
        anim.SetTrigger("Shot");
    }

    protected abstract void Shot(Vector3 p_firePos, Vector3 p_dir);

    public bool CheckCanShot() { return curBullet > 0; }

    public void Reload() { curBullet = maxBullet; }
}
