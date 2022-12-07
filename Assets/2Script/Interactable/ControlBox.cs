using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ControlBox : MonoBehaviour, IInteractable
{
    [SerializeField] private InGameManager manager = null;
    [SerializeField] private Mission mission = null;
    [SerializeField] private Highlighted highlighted = null;
    [SerializeField] private Outline outline = null;

    public void Use()
    {
        if (!manager.StartMission(mission)) return;
        outline.enabled = false;
        highlighted.enabled = false;
    }
}
