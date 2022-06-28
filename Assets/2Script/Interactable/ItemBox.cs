using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemBox : Interactable
{
    public MiniMapObject miniMapObject;

    public override void Use()
    {
        int t_random = Random.Range(1, 3);

        for (int i = 0; i < t_random; i++)
            SpawnItem();

        ItemManager.instance.AddPosition(transform.position);
        MiniMapManager.ReturnObject(miniMapObject);
        ObjectPooling.ReturnObject(gameObject);
    }

    private void SpawnItem()
    {
        int t_randomNum = Random.Range(0, 13);
        Item t_item = ObjectPooling.SpawnObject("Item", Vector3.zero, Quaternion.identity).GetComponent<Item>();
        switch (t_randomNum)
        {
            case 0:
            case 1:
                // ammo 12
                t_item.SetItem(EItemType.AMMO12, Random.Range(8, 17), transform.position);
                break;
            case 2:
            case 3:
            case 4:
                // ammo 7
                t_item.SetItem(EItemType.AMMO7, Random.Range(30, 61), transform.position);
                break;
            case 5:
            case 6:
            case 7:
                // ammo 5
                t_item.SetItem(EItemType.AMMO5, Random.Range(30, 61), transform.position);
                break;
            case 8:
            case 9:
            case 10:
                t_item.SetItem(EItemType.AMMO9, Random.Range(15, 21), transform.position);
                break;
            case 11:
                // heal
                t_item.SetItem(EItemType.HEAL, Random.Range(5, 11), transform.position);
                break;
            case 12:
                // grenade
            default:
                // weapon 
                t_item.SetItem(EItemType.WEAPON, Random.Range(0, (int)EWeaponType.END), transform.position);
                break;
        }
    }
}
