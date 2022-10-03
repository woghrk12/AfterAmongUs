using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon : MonoBehaviour
{
    [SerializeField] private Transform triggerPosition = null;
    [SerializeField] private Transform firePosition = null;
    [SerializeField] private float range = 0f;

    [SerializeField] private GameObject bullet = null; 

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.F1))
            UseWeapon();
    }

    private void UseWeapon()
    {
        Shot(firePosition.position, firePosition.position - triggerPosition.position);
    }

    private void Shot(Vector3 p_firePos, Vector3 p_dir)
    {
        var t_bullet = Instantiate(bullet, Vector3.zero, Quaternion.identity).GetComponent<Bullet>();
        t_bullet.InitValue(p_firePos, p_dir);
    }
}
