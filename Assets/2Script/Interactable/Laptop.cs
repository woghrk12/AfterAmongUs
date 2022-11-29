using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Laptop : MonoBehaviour, IInteractable
{
    public void Use() => UIManager.Instance.TitleUI.OnClickUseButton();
}
