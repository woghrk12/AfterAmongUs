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
    }

    private void Start()
    {
        moveController.SetControlType(GameManager.controlType);
    }

    public void SpawnPlayer(bool p_isLeft)
    {
        spriteRenderer.enabled = true;
        moveController.IsLeft = p_isLeft;
        
        anim.SetTrigger("Spawn");
    }
}
