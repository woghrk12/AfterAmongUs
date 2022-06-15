using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Laptop : MonoBehaviour
{
    [SerializeField] private GameObject colorSelectPanel;

    public void Use()
    {
        colorSelectPanel.SetActive(true);
    }
}
