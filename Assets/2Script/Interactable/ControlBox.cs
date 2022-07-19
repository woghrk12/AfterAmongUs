using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ControlBox : MonoBehaviour, IInteractable
{
    private BoxCollider2D boxCollider;
    private Mission mission;

    private void Awake()
    {
        boxCollider = GetComponent<BoxCollider2D>();
        mission = GetComponentInParent<Mission>();
    }

    public void Use()
    {
        if (!SetMission(mission)) return;

        boxCollider.enabled = false;
        mission.StartMission();
    }

    private bool SetMission(Mission p_mission)
    {
        if (InGameManager.missionInProgress == null)
        {
            InGameManager.missionInProgress = p_mission;
            return true;
        }
        return false;
    }
}
