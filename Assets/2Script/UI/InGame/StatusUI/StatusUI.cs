using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StatusUI : MonoBehaviour
{
    [SerializeField] private ControlStatus bulletStatus = null;

    public void InitUI(Weapon p_weapon)
    {
        SetMaxBullet(p_weapon.MaxBullet);
        SetBullet(p_weapon.CurBullet);
    }

    public void SetMaxBullet(int p_value)
    {
        bulletStatus.MaxValue = p_value;
    }

    public void SetBullet(int p_value)
    {
        bulletStatus.SetValue(p_value);
    }
}
