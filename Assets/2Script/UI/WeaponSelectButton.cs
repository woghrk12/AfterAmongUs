using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class WeaponSelectButton : MonoBehaviour
{
    [SerializeField] private PlayerWeapon weapon;

    [SerializeField] private Image weaponImage;
    private Button button;

    private void Awake()
    {
        button = GetComponent<Button>();
    }

    public void OnClick()
    { 
        
    }
}
