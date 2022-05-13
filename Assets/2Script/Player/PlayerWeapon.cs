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
    [SerializeField] private float recoil;
    private float cameraRecoil;

    private void Awake()
    {
        anim = GetComponent<Animator>();
        mainCamera = Camera.main.GetComponent<MainCamera>();

        cameraRecoil = recoil * 0.2f;
    }
    
    public void Shot()
    {
        StartCoroutine(ShotFlash());

        var t_recoil = Random.Range(-recoil, recoil);
        firePosition.Rotate(0f, 0f, t_recoil);
        Instantiate(bulletPrefab, firePosition.position, firePosition.rotation);
        firePosition.Rotate(0f, 0f, -t_recoil);

        anim.SetTrigger("Shot");
        mainCamera.SetCameraShake(cameraRecoil, 0.1f);
    }

    private IEnumerator ShotFlash()
    {
        muzzleFlash.SetActive(true);
        yield return new WaitForSeconds(0.01f);
        muzzleFlash.SetActive(false);
    }
}

