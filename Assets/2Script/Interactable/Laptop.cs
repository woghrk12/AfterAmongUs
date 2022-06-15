using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Laptop : MonoBehaviour
{
    [SerializeField] private ColorSelectPanel colorSelectPanel;

    public void Use()
    {
        colorSelectPanel.gameObject.SetActive(true);
        colorSelectPanel.EnablePanel();
    }
}
