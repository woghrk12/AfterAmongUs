using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ControlBox : MonoBehaviour, IInteractable
{
    private InGameManager inGameManager = null;

    private BoxCollider2D boxCollider;
    private Mission mission;

    private void Awake()
    {
        boxCollider = GetComponent<BoxCollider2D>();
        mission = GetComponentInParent<Mission>();
    }

    private void Start()
    {
        inGameManager = InGameManager.instance;
    }

    public void Use()
    {
        if (!inGameManager.StartMission(mission)) return;

        boxCollider.enabled = false;
    }
}
