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
    public EItemType itemType;
    public int num;
}

public enum EScene { TITLE, INGAME, LOADING }
public enum EBulletType { FIVEMM, SEVENMM, TWELVEGAUGE }
public enum EWeaponType { RIFLE, SHOTGUN }
public enum EItemType { AMMO5, AMMO7, AMMO12, HEAL, GRENADE, WEAPON };
public enum EPlayerColor { RED, BLUE, GREEN, YELLOW, ORANGE, PURPLE, CYAN, BROWN, PINK, WHITE, BLACK, GRAY }