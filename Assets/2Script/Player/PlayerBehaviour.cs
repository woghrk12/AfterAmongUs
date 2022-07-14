using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerBehaviour : MonoBehaviour
{
    protected Animator anim;
    protected SpriteRenderer spriteRenderer;

    protected CharacterMove moveController;

    public bool CanMove { set { moveController.CanMove = value; } }
    public bool IsLeft { set { moveController.IsLeft = value; } }

    private void Awake()
    {
        anim = GetComponent<Animator>();
        spriteRenderer = GetComponentInChildren<SpriteRenderer>();

        var t_instMat = Instantiate(spriteRenderer.material);
        spriteRenderer.material = t_instMat;

        moveController = GetComponent<CharacterMove>();
    }

    public void SetColor(EPlayerColor p_color)
    {
        spriteRenderer.material.SetColor("_PlayerColor", PlayerColor.GetColor(p_color));
    }

    protected void SetAlpha(float p_value)
    {
        var t_color = spriteRenderer.color;
        t_color.a = p_value;
        spriteRenderer.color = t_color;
    }
}
