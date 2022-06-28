using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Reactor : Interactable
{
    private Animator anim;
    private BoxCollider2D boxCollider;

    [SerializeField] private Region region;

    private void Awake()
    {
        anim = GetComponent<Animator>();
        boxCollider = GetComponent<BoxCollider2D>();
    }

    public override void Use()
    {
        InGameManager.SetPlayerRegion(region);
        anim.SetTrigger("isActivated");
        boxCollider.enabled = false;
    }
}
