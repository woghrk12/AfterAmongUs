using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ControlBox : MonoBehaviour, IInteractable
{
    private InGameManager inGameManager = null;
    
    private Mission mission = null;
    
    private BoxCollider2D boxCollider = null;
    [SerializeField] private BoxCollider2D highlightedCollider = null;
    [SerializeField] private BoxCollider2D outLineCollider = null;

    private void Awake()
    {
        mission = GetComponentInParent<Mission>();
        boxCollider = GetComponent<BoxCollider2D>();
    }

    private void Start()
    {
        inGameManager = InGameManager.instance;
    }

    public void Use()
    {
        if (!inGameManager.StartMission(mission)) return;

        boxCollider.enabled = false;
        highlightedCollider.enabled = false;
        outLineCollider.enabled = false;    
    }
}
