using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class WeaponSelectButton : MonoBehaviour
{
    [SerializeField] private Image weaponImage = null;
    [SerializeField] private Button selectButton = null;


    public Image WeaponImage { get { return weaponImage; } }
    public Button SelectButton { get { return selectButton; } }
}
