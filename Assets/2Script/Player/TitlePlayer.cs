using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TitlePlayer : PlayerBehaviour
{
    private Animator anim = null;
    private SpriteRenderer spriteRenderer = null;

    protected override void Awake()
    {
        base.Awake();

        anim = GetComponent<Animator>();
        spriteRenderer = GetComponentInChildren<SpriteRenderer>();
    }
    public void SpawnPlayer(bool p_isLeft)
    {
        moveController.IsLeft = p_isLeft;

        spriteRenderer.enabled = true;
        anim.SetTrigger("Spawn");
    }
}
