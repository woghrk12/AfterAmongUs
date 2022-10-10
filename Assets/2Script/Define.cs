using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

// Structure
[Serializable]
public struct Pool
{
    public string tag;
    public GameObject prefab;
    public int size;
}

// Enum
public enum EScene { TITLE, INGAME, LOADING }
public enum ESortingType { STATIC, UPDATE }
public enum EPlayerColor { RED, BLUE, GREEN, PINK, ORANGE, YELLOW, BLACK, WHITE, PURPLE, BROWN, CYAN, LIME, END }
public enum EPlayerWeapon { RIFLE, SHOTGUN, PISTOL, SNIPER, MACHINEGUN, SUBMACHINEGUN, NONE }
public enum ELayer { DEFAULT, TRANSPARENTFX, IGNORERAYCAST, MAP, WATER, UI, MOVEMENTCOLLIDER, HITBOX, HITBOXTRIGGER, ENEMYHITBOX, END }

// Interface
public interface IInteractable
{
    public void Use();
}