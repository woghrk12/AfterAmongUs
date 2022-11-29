using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class WeaponSelectPanel : MonoBehaviour
{
    [SerializeField] private WeaponSelectButton[] previewWeapon = null;
    [SerializeField] private List<WeaponSelectButton> weaponSelectButtons = null;

    private void OnEnable()
    {
        var t_weapon = GameManager.playerWeapon;
        for (int i = 0; i < t_weapon.Length; i++)
            SetPreviewImage(previewWeapon[i], t_weapon[i]);
    }

    public void OnClickWeaponButton(int p_weapon)
    {
        var t_playerWeapon = GameManager.playerWeapon;

        for (int i = 0; i < t_playerWeapon.Length; i++)
        {
            if (t_playerWeapon[i] != EPlayerWeapon.NONE) continue;

            GameManager.playerWeapon[i] = (EPlayerWeapon)p_weapon;
            weaponSelectButtons[p_weapon].SelectButton.interactable = false;
            SetPreviewImage(previewWeapon[i], (EPlayerWeapon)p_weapon);
            break;
        }
    }

    public void OnClickPreviewButton(int p_numButton)
    {
        weaponSelectButtons[(int)GameManager.playerWeapon[p_numButton]].SelectButton.interactable = true;
        GameManager.playerWeapon[p_numButton] = EPlayerWeapon.NONE;
        SetPreviewImage(previewWeapon[p_numButton], EPlayerWeapon.NONE);
        previewWeapon[p_numButton].SelectButton.interactable = false;
    }

    private void SetPreviewImage(WeaponSelectButton p_button, EPlayerWeapon p_weapon)
    {
        var t_image = p_button.WeaponImage;
        var t_button = p_button.SelectButton;

        if (p_weapon == EPlayerWeapon.NONE)
        {
            t_image.enabled = false;
            t_button.interactable = false;
            return;
        }

        if (!t_button.interactable) t_button.interactable = true;
        if (!t_image.enabled) t_image.enabled = true;

        var t_weaponImage = weaponSelectButtons[(int)p_weapon].WeaponImage;
        t_image.sprite = t_weaponImage.sprite;
        t_image.rectTransform.localScale = t_weaponImage.rectTransform.localScale;
    }
}
