using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterColor : MonoBehaviour
{
    [SerializeField] private SpriteRenderer spriteRenderer = null;

    private void Awake()
    {
        var t_instMat = Instantiate(spriteRenderer.material);
        spriteRenderer.material = t_instMat;
    }

    public void SetColor(EPlayerColor p_color)
    {
        spriteRenderer.material.SetColor("_PlayerColor", PlayerColor.GetColor(p_color));
    }

    public void SetAlpha(float p_value)
    {
        var t_color = spriteRenderer.color;

        if (p_value < 0f || p_value > 1f) p_value = Mathf.Clamp(p_value, 0f, 1f);

        t_color.a = p_value;
        spriteRenderer.color = t_color;
    }
}
