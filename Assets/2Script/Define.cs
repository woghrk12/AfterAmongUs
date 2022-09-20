using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Enum
public enum ESortingType { STATIC, UPDATE }
public enum EPlayerColor { RED, BLUE, GREEN, PINK, ORANGE, YELLOW, BLACK, WHITE, PURPLE, BROWN, CYAN, LIME, END }

// Interface

public interface IInteractable
{
    public void Use();
}