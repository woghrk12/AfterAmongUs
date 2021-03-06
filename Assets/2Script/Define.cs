using System;
using System.Collections.Generic;
using UnityEngine;

// Structure
[Serializable]
public struct Pool
{
    public string tag;
    public GameObject prefab;
    public int size;
}

// Enum
public enum EScene { TITLE, INGAME, LOADING, ENDING, END }
public enum EBulletType { FIVEMM, SEVENMM, TWELVEGAUGE, NINEMM, END }
public enum EWeaponType { RIFLE, SHOTGUN, PISTOL, END }
public enum EItemType { AMMO5, AMMO7, AMMO12, AMMO9, HEAL, GRENADE, WEAPON, END }
public enum EPlayerColor { RED, BLUE, GREEN, YELLOW, ORANGE, PURPLE, CYAN, BROWN, PINK, WHITE, BLACK, GRAY, END }
public enum ELayer { 
    DEFAULT, TRANSPARENTFX, IGNORERAYCAST, MAP, WATER, UI, PLAYER, PLAYERONHIT, 
    PLAYERDIE, ENEMY, ENEMYDIE, BULLET, ENEMYBULLET, COLLIDER, RADER, ENEMYRADER, 
    CAMERACOLLIDER, END 
}
public enum EMiniMapObject { ITEMBOX, ENEMYPORTAL, END }
public enum EControlType { JOYSTICK, KEYBOARD, END }

// Interface
public interface IInteractable
{
    void Use();
}
public interface IEffect
{
    void StartEffect();
    void StopEffect();
}