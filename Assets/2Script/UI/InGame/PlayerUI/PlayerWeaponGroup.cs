using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerWeaponGroup : MonoBehaviour
{
    [SerializeField] private GamePlayer player = null;
    [SerializeField] private ControlSlider[] remainBullets = null;

    public void SetMaxValue(int p_index, int p_value) => remainBullets[p_index].SetMaxValue(p_value);
    public void SetValue(int p_index, int p_value) => remainBullets[p_index].SetValue(p_value);
    public void OnClickWeaponButton(int p_index)
    {
        for (int i = 0; i < remainBullets.Length; i++)
            remainBullets[i].gameObject.SetActive(i == p_index);

        player.Swap(p_index);
    } 
}
