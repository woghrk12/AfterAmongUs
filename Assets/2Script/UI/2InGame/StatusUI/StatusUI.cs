using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StatusUI : MonoBehaviour
{
    [SerializeField] private ControlSlider healthStatus = null;
    [SerializeField] private ControlSlider bulletStatus = null;

    public ControlSlider HealthStatus { get { return healthStatus; } }
    public ControlSlider BulletStatus { get { return bulletStatus; } }


    public void SetHealthStatus(Damagable p_health)
    {
        p_health.HealthBar = healthStatus;
    }

    public void SetBulletStatus(Weapon p_weapon)
    {
        bulletStatus.SetMaxValue(p_weapon.MaxBullet);
        bulletStatus.SetValue(p_weapon.CurBullet);
    }
}
