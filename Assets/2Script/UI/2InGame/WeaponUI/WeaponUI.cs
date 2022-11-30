using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class WeaponUI : MonoBehaviour
{
    [SerializeField] private Button[] weaponButtons = null; 
    [SerializeField] private GameObject[] weaponImages = null;
    [SerializeField] private Button reloadButton = null;
    [SerializeField] private Image reloadImage = null;

    public void InitUI(EPlayerWeapon[] p_weapons)
    {
        SetWeaponImage(p_weapons);
        SetSwapButton(0);
    }

    private void SetWeaponImage(EPlayerWeapon[] p_weapons)
    {
        for (int i = 0; i < weaponImages.Length; i++)
        {
            if (i.Equals((int)p_weapons[0]))
            {
                weaponImages[i].SetActive(true);
                weaponImages[i].transform.position = weaponButtons[0].transform.position;
                continue;
            }
            else if (i.Equals((int)p_weapons[1]))
            {
                weaponImages[i].SetActive(true);
                weaponImages[i].transform.position = weaponButtons[1].transform.position;
                continue;
            }

            weaponImages[i].SetActive(false);
        }
    }

    public void SetSwapButton(int p_idx)
    {
        for (int i = 0; i < weaponButtons.Length; i++)
            weaponButtons[i].interactable = p_idx != i;
    }

    public void SetReloadButton(bool p_isActive)
    {
        reloadButton.interactable = p_isActive;
        reloadImage.color = p_isActive ? reloadButton.colors.normalColor : reloadButton.colors.disabledColor;
    }
}
