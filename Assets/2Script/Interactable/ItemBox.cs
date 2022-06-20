using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemBox : MonoBehaviour
{
    public MiniMapObject miniMapObject;

    private Item item;

    public Item Use()
    {
        int t_randomNum = Random.Range(0, 13);

        switch (t_randomNum)
        {
            case 0:
            case 1:
                // ammo 12
                item.itemType = EItemType.AMMO12;
                item.num = Random.Range(8, 17);
                break;
            case 2:
            case 3:
            case 4:
                // ammo 7
                item.itemType = EItemType.AMMO7;
                item.num = Random.Range(30, 61);
                break;
            case 5:
            case 6:
            case 7:
                // ammo 5
                item.itemType = EItemType.AMMO5;
                item.num = Random.Range(30, 61);
                break;
            case 8:
            case 9:
            case 10:
                item.itemType = EItemType.AMMO9;
                item.num = Random.Range(15, 21);
                break;
            case 11:
                // heal
                item.itemType = EItemType.HEAL;
                item.num = Random.Range(5, 11);
                break;
            case 12:
                // grenade
                item.itemType = EItemType.GRENADE;
                item.num = Random.Range(1, 3);
                break;
            default:
                // weapon 
                item.itemType = EItemType.WEAPON;
                item.num = Random.Range(1, 3);
                break;
        }

        ItemManager.instance.AddPosition(transform.position);
        MiniMapManager.ReturnObject(miniMapObject);
        ObjectPooling.ReturnObject(gameObject);

        return item;
    }
}
