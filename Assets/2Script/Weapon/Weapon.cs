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
    
    public float Range { get { return range; } }

    [SerializeField] private GameObject bullet = null;

    private void Awake()
    {
        anim.SetFloat("motionSpeed", 1.5f - recoilPower);
    }

    public void UseWeapon()
    {
        Shot(firePosition.position, firePosition.position - triggerPosition.position);
    }

    private void Shot(Vector3 p_firePos, Vector3 p_dir)
    {
        var t_bullet = Instantiate(bullet, Vector3.zero, Quaternion.identity).GetComponent<Bullet>();
        t_bullet.InitValue(p_firePos, p_dir);
        anim.SetTrigger("Shot");
    }
}
