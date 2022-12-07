using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ControlBox : MonoBehaviour, IInteractable
{
    [SerializeField] private InGameManager manager = null;
    [SerializeField] private Mission mission = null;
    [SerializeField] private GameObject highlighted = null;
    [SerializeField] private GameObject outline = null;

    public void Use()
    {
        if (!manager.StartMission(mission)) return;

        outline.gameObject.SetActive(false);
        highlighted.gameObject.SetActive(false);
    }
}
