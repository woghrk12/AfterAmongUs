using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SectorBullet : Bullet
{
    [SerializeField] private string hitEffect = null;

    protected override void OnHit(Vector3 p_dir, RaycastHit2D p_hitInfo)
    {
        DealDamage(p_hitInfo.transform, damage);

        ObjectPoolingManager.SpawnObject(hitEffect, p_hitInfo.point, Quaternion.AngleAxis(Mathf.Atan2(p_dir.y, p_dir.x) * Mathf.Rad2Deg, Vector3.forward));
        
        var t_target = Physics2D.OverlapCircleAll(p_hitInfo.point, 2f, 1 << (int)targetLayer);
        for (int i = 0; i < t_target.Length; i++)
        {
            if (t_target[i].Equals(p_hitInfo.collider)) continue;
            var t_dot = Vector3.Dot(((Vector2)t_target[i].transform.position - p_hitInfo.point).normalized, p_dir.normalized);
            var t_degree = Mathf.Acos(t_dot) * Mathf.Rad2Deg;
            if (t_degree > 20f) continue;
            DealDamage(t_target[i].transform, damage);
        }
    }
}
