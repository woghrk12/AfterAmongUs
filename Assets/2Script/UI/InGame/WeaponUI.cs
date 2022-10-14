using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class WeaponUI : MonoBehaviour
{
    [SerializeField] private Button[] weaponButtons = null; 
    [SerializeField] private GameObject[] weaponImages = null;

    private void Start()
    {
        var t_weapon = GameManager.playerWeapon;
        for (int i = 0; i < weaponImages.Length; i++)
        {
            if (i.Equals((int)t_weapon[0]))
            {
                weaponImages[i].SetActive(true);
                weaponImages[i].transform.position = weaponButtons[0].transform.position;
                weaponButtons[0].interactable = false;
                continue;
            }
            if (i.Equals((int)t_weapon[1]))
            {
                weaponImages[i].SetActive(true);
                weaponImages[i].transform.position = weaponButtons[1].transform.position;
                weaponButtons[1].interactable = true;
                continue;
            }
            weaponImages[i].SetActive(false);
        }
    }
}
