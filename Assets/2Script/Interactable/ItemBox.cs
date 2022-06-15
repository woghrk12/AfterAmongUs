using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemBox : MonoBehaviour
{
    public MiniMapObject miniMapObject;

    private Item item;

    public Item Use()
    {
        int t_randomNum = Random.Range(0, 10);

        switch (t_randomNum)
        {
            case 0:
            case 1:
                // ammo 12
                item.itemType = ItemType.AMMO12;
                item.num = Random.Range(10, 21);
                break;
            case 2:
            case 3:
            case 4:
                // ammo 7
                item.itemType = ItemType.AMMO7;
                item.num = Random.Range(30, 61);
                break;
            case 5:
            case 6:
            case 7:
                // ammo 5
                item.itemType = ItemType.AMMO5;
                item.num = Random.Range(30, 61);
                break;
            case 8:
                // heal
                item.itemType = ItemType.HEAL;
                item.num = Random.Range(5, 11);
                break;
            case 9:
                // grenade
                item.itemType = ItemType.GRENADE;
                item.num = Random.Range(1, 3);
                break;
            default:
                // weapon 
                item.itemType = ItemType.WEAPON;
                item.num = Random.Range(1, 3);
                break;
        }

        ItemManager.instance.AddPosition(transform.position);
        MiniMapManager.ReturnObject(miniMapObject);
        ObjectPooling.ReturnObject(gameObject);

        return item;
    }
}
