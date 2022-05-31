using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemBox : MonoBehaviour
{
    public MiniMapObject miniMapObject;

    public void Use()
    {
        int randomNum = Random.Range(0, 10);

        switch (randomNum)
        {
            case 0:
            case 1:
            case 2:
            case 3:
            case 4:
            case 5:
            case 6:
            case 7:
            case 8:
            case 9:

            default:
                break;
        }

        ItemManager.instance.AddPosition(transform.position);
        MiniMapManager.ReturnObject(miniMapObject);
        ObjectPooling.ReturnObject(gameObject);
    }
}
