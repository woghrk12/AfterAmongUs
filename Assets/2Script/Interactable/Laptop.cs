using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Laptop : MonoBehaviour, IInteractable
{
    [SerializeField] private TitleUIManager titleUIManager = null;

    public void Use()
    {
        titleUIManager.OnCustomUI();
    }
}
