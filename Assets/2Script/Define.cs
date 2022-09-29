using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Enum
public enum EScene { TITLE, INGAME, LOADING }
public enum ESortingType { STATIC, UPDATE }
public enum EPlayerColor { RED, BLUE, GREEN, PINK, ORANGE, YELLOW, BLACK, WHITE, PURPLE, BROWN, CYAN, LIME, END }
public enum EPlayerWeapon { RIFLE, SHOTGUN, PISTOL, SNIPER, MACHINEGUN, SUBMACHINEGUN, NONE }
public enum ELayer { DEFAULT, TRANSPARENTFX, IGNORERAYCAST, MAP, WATER, UI, MOVEMENTCOLLIDER, HITBOX, HITBOXTRIGGER, END }

// Interface
public interface IInteractable
{
    public void Use();
}