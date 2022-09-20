using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Enum
public enum ESortingType { STATIC, UPDATE }

// Interface

public interface IInteractable
{
    public void Use();
}