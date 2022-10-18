using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class WeaponUI : MonoBehaviour
{
    [SerializeField] private GamePlayer gamePlayer = null;
    [SerializeField] private Button[] weaponButtons = null; 
    [SerializeField] private GameObject[] weaponImages = null;
    [SerializeField] private Button reloadButton = null;
    [SerializeField] private Image reloadImage = null;

    private void Start()
    {
        var t_weapon = GameManager.playerWeapon;
        weaponButtons[0].interactable = false;
        weaponButtons[1].interactable = true;

        for (int i = 0; i < weaponImages.Length; i++)
        {
            if (i.Equals((int)t_weapon[0]))
            {
                weaponImages[i].SetActive(true);
                weaponImages[i].transform.position = weaponButtons[0].transform.position;
                continue;
            }
            if (i.Equals((int)t_weapon[1]))
            {
                weaponImages[i].SetActive(true);
                weaponImages[i].transform.position = weaponButtons[1].transform.position;
                continue;
            }

            weaponImages[i].SetActive(false);
        }
    }

    public void OnClickReloadButton()
    {
        StartCoroutine(ReloadWeapon());
    }

    private IEnumerator ReloadWeapon()
    {
        gamePlayer.IsReload = true;
        reloadButton.interactable = false;
        reloadImage.color = reloadButton.colors.disabledColor;

        yield return gamePlayer.Reload();
        
        reloadButton.interactable = true;
        reloadImage.color = reloadButton.colors.normalColor;
        gamePlayer.IsReload = false;
    }
}
