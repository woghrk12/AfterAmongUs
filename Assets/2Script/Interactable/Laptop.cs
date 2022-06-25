using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Laptop : Interactable
{
    public override void Use()
    {
        TitleManager.TurnOnColorSelectPanel();
    }
}
