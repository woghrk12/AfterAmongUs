using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public struct Pool
{
    public string tag;
    public GameObject prefab;
    public int size;
}

[Serializable]
public struct Item
{
    public ItemType itemType;
    public int num;
}

public enum BulletType { FIVEMM, SEVENMM, TWELVEGAUGE }
public enum WeaponType { RIFLE, SHOTGUN }
public enum ItemType { AMMO5, AMMO7, AMMO12, HEAL, GRENADE, WEAPON };