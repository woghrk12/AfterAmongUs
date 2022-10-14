using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MultiFire : MonoBehaviour
{
    [SerializeField] private int bulletNum = 0;
    public void Fire(string p_bullet, Vector3 p_firePos, Vector3 p_dir, float p_accurate)
        => ShotBullet(p_bullet, p_firePos, p_dir, p_accurate);

    private void ShotBullet(string p_bullet, Vector3 p_firePos, Vector3 p_dir, float p_accurate)
    {
        for (int i = 0; i < bulletNum; i++)
        {
            var t_bullet = ObjectPoolingManager.SpawnObject(p_bullet).GetComponent<Bullet>();
            var t_dir = Quaternion.AngleAxis(Random.Range(-p_accurate, p_accurate), Vector3.forward) * p_dir;
            t_bullet.InitValue(p_firePos, t_dir);
        }
    }
}
