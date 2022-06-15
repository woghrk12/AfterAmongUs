using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TitlePlayer : PlayerBehaviour
{
    private bool uDown;

    private void Awake()
    {
        anim = GetComponent<Animator>();
        spriteRenderer = GetComponentInChildren<SpriteRenderer>();

        var t_instMat = Instantiate(spriteRenderer.material);
        spriteRenderer.material = t_instMat;

        canMove = false;
    }

    protected override void Update()
    {
        base.Update();
    }

    protected override void FixedUpdate()
    {
        base.FixedUpdate();
    }

    protected override void GetInput()
    {
        base.GetInput();

        uDown = Input.GetButtonDown("Use");
    }

    protected override void Move()
    {
        base.Move();

        if(moveDir.x != 0f)
            spriteRenderer.flipX = moveDir.x < 0;
    }

    public void SpawnPlayer(bool p_isFlipX)
    {
        spriteRenderer.enabled = true;
        spriteRenderer.flipX = p_isFlipX;
        
        anim.SetTrigger("Spawn");
    }

    private void Use() 
    {
        if (!uDown) return;

        canMove = false;

    }
}
