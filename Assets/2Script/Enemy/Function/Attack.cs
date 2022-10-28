using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Attack : MonoBehaviour
{
    [SerializeField] private string bullet = null;

    public void Fire(Vector3 p_firePos, Vector3 p_dir) => ShotBullet(bullet, p_firePos, p_dir);

    private void ShotBullet(string p_bullet, Vector3 p_firePos, Vector3 p_dir)
    {
        var t_bullet = ObjectPoolingManager.SpawnObject(p_bullet).GetComponent<Bullet>();
        t_bullet.InitValue(p_firePos, p_dir);
    }
}
