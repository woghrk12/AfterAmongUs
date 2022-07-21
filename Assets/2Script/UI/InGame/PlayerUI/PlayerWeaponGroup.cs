using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerWeaponGroup : MonoBehaviour
{
    [SerializeField] private GamePlayer player = null;
    [SerializeField] private Image[] weaponButtons = null;

    private void Start()
    {
        OnClickWeaponButton(0);
    }

    public void OnClickWeaponButton(int p_index)
    {
        player.Swap(p_index);

        for (int i = 0; i < weaponButtons.Length; i++)
        {
            weaponButtons[i].color = (i == p_index) ? Color.green : Color.white;
        }
    }
}
