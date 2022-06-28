using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Reactor : Interactable
{
    [SerializeField] private Region region;

    public override void Use()
    {
        InGameManager.instance.SetPlayerRegion(region);
        Debug.Log("Reactor");
    }
}
