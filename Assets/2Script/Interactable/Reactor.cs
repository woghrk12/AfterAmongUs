using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Reactor : MonoBehaviour, IInteractable, IMission
{
    private Animator anim;
    private BoxCollider2D boxCollider;

    [SerializeField] private Region region;

    [SerializeField] private float completeTime;
    private bool isComplete;

    private bool IsComplete 
    {
        set 
        {
            isComplete = value;
            if (value) SuccessMission();
        }
    }

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
        if (!InGameManager.SetMission(this)) return;
    
        StartMission();
    }

    public void StartMission()
    {
        StartCoroutine(ChangeLight());
    }

    private IEnumerator ChangeLight()
    {
        yield return InGameUIManager.FadeOut();

        InGameManager.SetPlayerRegion(region);
        
        anim.SetBool("isActivated", true);
        boxCollider.enabled = false;

        InGameManager.TurnOnPointLight();

        yield return InGameUIManager.FadeIn();
    }

    public bool SuccessMission()
    {
        return true;
    }

    public bool FailMission()
    {

        return false;
    }

}
