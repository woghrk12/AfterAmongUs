using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Reactor : MonoBehaviour, IInteractable, IMission
{
    private Animator anim;
    private BoxCollider2D boxCollider;

    [SerializeField] private Region region;

    private void Awake()
    {
        anim = GetComponent<Animator>();
        boxCollider = GetComponent<BoxCollider2D>();
    }

    public void Use()
    {
        InGameManager.SetPlayerRegion(region);
        anim.SetBool("isActivated", true);
        boxCollider.enabled = false;
    }

    public IMission StartMission()
    {
        return this;
    }

    public bool EndMission()
    {
        return false;
    }
}
