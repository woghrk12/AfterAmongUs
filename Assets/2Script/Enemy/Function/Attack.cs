using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Attack : MonoBehaviour
{
    [SerializeField] private string bullet = null;

    public bool CheckCanAttack(Transform p_target, float p_attackRange)
    {
        var t_origin = transform.position;
        var t_target = p_target.position;
        var t_hit = Physics2D.Raycast(t_origin, t_target - t_origin, p_attackRange, 1 << (int)ELayer.MAP | 1 << (int)ELayer.HITBOX);
        
        if (t_hit.collider.gameObject.layer.Equals((int)ELayer.MAP)) return false;
        return true;
    }

    public void Fire(Vector3 p_firePos, Vector3 p_dir) => ShotBullet(bullet, p_firePos, p_dir);

    private void ShotBullet(string p_bullet, Vector3 p_firePos, Vector3 p_dir)
    {
        var t_bullet = ObjectPoolingManager.SpawnObject(p_bullet).GetComponent<Bullet>();
        t_bullet.InitValue(p_firePos, p_dir);
    }
}
