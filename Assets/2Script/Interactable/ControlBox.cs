using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ControlBox : MonoBehaviour, IInteractable
{
    [SerializeField] private InGameManager manager = null;
    [SerializeField] private Mission mission = null;
    [SerializeField] private Highlighted highlighted = null;

    public void Use()
    {
        StartCoroutine(manager.StartMission(mission));
        highlighted.enabled = false;
    }
}
