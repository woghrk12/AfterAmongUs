using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SingleBullet : Bullet
{
    [SerializeField] private string hitEffect = null;

    protected override void OnHit(RaycastHit2D p_hitInfo)
    {
        var t_normal = p_hitInfo.normal;
        var t_angle = Mathf.Atan2(t_normal.y, t_normal.x) * Mathf.Rad2Deg;

        ObjectPoolingManager.SpawnObject(hitEffect, p_hitInfo.point, Quaternion.AngleAxis(t_angle, Vector3.forward));
    }
}
