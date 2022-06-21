using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MuzzleFlash : MonoBehaviour
{
    [SerializeField] private GameObject muzzleFlash;

    private void OnEnable()
    {
        StartCoroutine(ShotFlash());
    }

    private IEnumerator ShotFlash()
    {
        muzzleFlash.SetActive(true);
        yield return new WaitForSeconds(0.05f);
        muzzleFlash.SetActive(false);

        ObjectPooling.ReturnObject(gameObject);
    }
}
