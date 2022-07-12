using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TitlePlayer : PlayerBehaviour
{
    private void Awake()
    {
        anim = GetComponent<Animator>();
        spriteRenderer = GetComponentInChildren<SpriteRenderer>();

        var t_instMat = Instantiate(spriteRenderer.material);
        spriteRenderer.material = t_instMat;

        moveController = GetComponent<CharacterMove>();

        canUseObject = new List<GameObject>();
    }

    public void SpawnPlayer(bool p_isFlipX)
    {
        spriteRenderer.enabled = true;
        spriteRenderer.flipX = p_isFlipX;
        
        anim.SetTrigger("Spawn");
    }
}
