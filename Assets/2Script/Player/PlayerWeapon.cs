using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerWeapon : MonoBehaviour
{
    private Animator anim;
    private MainCamera mainCamera;

    [SerializeField] private Transform firePosition;
    [SerializeField] private GameObject bulletPrefab;
    [SerializeField] private GameObject muzzleFlash;
    public float rate;
    public float recoil;

    private void Awake()
    {
        anim = GetComponent<Animator>();
        mainCamera = Camera.main.GetComponent<MainCamera>();
    }
    
    public void Shot()
    {
        StartCoroutine(ShotFlash());
        Instantiate(bulletPrefab, firePosition.position, firePosition.rotation);
        anim.SetTrigger("Shot");
        mainCamera.SetCameraShake(recoil, 0.1f);
    }

    private IEnumerator ShotFlash()
    {
        muzzleFlash.SetActive(true);
        yield return new WaitForSeconds(0.01f);
        muzzleFlash.SetActive(false);
    }
}

