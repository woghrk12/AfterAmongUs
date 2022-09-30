using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterColor : MonoBehaviour
{
    [SerializeField] private SpriteRenderer spriteRenderer = null;

    private void Awake()
    {
        var t_inst = Instantiate(spriteRenderer.material);
        spriteRenderer.material = t_inst;
    }

    public void SetColor(int p_color)
    {
        spriteRenderer.material.SetColor("_PlayerColor", PlayerColor.GetColor((EPlayerColor)p_color));
    }
}
