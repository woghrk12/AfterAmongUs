using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterColor : MonoBehaviour
{
    [SerializeField] private SpriteRenderer spriteRenderer = null;

    public void SetColor(int p_color)
    {
        spriteRenderer.material.SetColor("_PlayerColor", PlayerColor.GetColor((EPlayerColor)p_color));
    }
}
