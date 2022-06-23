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
public enum EBulletType { FIVEMM, SEVENMM, TWELVEGAUGE, NINEMM }
public enum EWeaponType { RIFLE, SHOTGUN, PISTOL }
public enum EItemType { AMMO5, AMMO7, AMMO12, AMMO9, HEAL, GRENADE, WEAPON };
public enum EPlayerColor { RED, BLUE, GREEN, YELLOW, ORANGE, PURPLE, CYAN, BROWN, PINK, WHITE, BLACK, GRAY }
public enum ELayer { 
    DEFAULT, TRANSPARENTFX, IGNORERAYCAST, MAP, WATER, UI, PLAYER, PLAYERONHIT, 
    PLAYERDIE, ENEMY, ENEMYDIE, BULLET, ENEMYBULLET, COLLIDER, RADER, ENEMYRADER 
}