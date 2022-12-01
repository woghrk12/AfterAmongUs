using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StatusUI : MonoBehaviour
{
    [SerializeField] private ControlStatus bulletStatus = null;

    public ControlStatus BulletStatus { get { return bulletStatus; } }

    public void InitUI(Weapon p_weapon)
    {
        bulletStatus.MaxValue = p_weapon.MaxBullet;
        bulletStatus.SetValue(p_weapon.CurBullet);
    }
}
