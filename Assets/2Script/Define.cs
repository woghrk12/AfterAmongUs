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

[Serializable]
public class Node
{
    public Node(bool p_isWall, int p_xIdx, int p_yIdx, float p_xPos, float p_yPos)
    {
        isWall = p_isWall;
        xIdx = p_xIdx; yIdx = p_yIdx;
        xPos = p_xPos; yPos = p_yPos;
    }

    public bool isWall;
    public Node parentNode;

    public int xIdx, yIdx;
    public float xPos, yPos;
    public int g, h;
    public int f { get { return g + h; } }
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