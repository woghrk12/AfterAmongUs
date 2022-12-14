using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StatusUI : MonoBehaviour
{
    [SerializeField] private ControlSlider healthStatus = null;
    [SerializeField] private ControlSlider bulletStatus = null;
    [SerializeField] private ControlSlider missionStatus = null;

    public ControlSlider HealthStatus { get { return healthStatus; } }
    public ControlSlider BulletStatus { get { return bulletStatus; } }
    public ControlSlider MissionStatus { get { return missionStatus; } }


    public void SetHealthStatus(Damagable p_health)
    {
        p_health.HealthBar = healthStatus;
    }

    public void SetBulletStatus(Weapon p_weapon)
    {
        bulletStatus.SetMaxValue(p_weapon.MaxBullet);
        bulletStatus.SetValue(p_weapon.CurBullet);
    }

    public void SetMissionStatus(int p_totalNum)
    {
        missionStatus.SetMaxValue(p_totalNum);
        missionStatus.SetValue(0);
    }
}
