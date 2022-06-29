using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Reactor : MonoBehaviour, IInteractable, IMission
{
    private Animator anim;
    private BoxCollider2D boxCollider;

    [SerializeField] private Region region;

    private float completeTime;

    [SerializeField] private ControlSlider controlSlider;
    [SerializeField] private int maxHealth;
    private int curHealth;

    private void Awake()
    {
        anim = GetComponent<Animator>();
        boxCollider = GetComponent<BoxCollider2D>();
    }

    private void Start()
    {
        
    }

    private void Update()
    {
        
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
