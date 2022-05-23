using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemBox : MonoBehaviour
{
    private SpriteRenderer spriteRenderer;

    private void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        spriteRenderer.material.SetFloat("_Highlighted", 1f);
    }
    
    private void OnTriggerExit2D(Collider2D collision)
    {
        spriteRenderer.material.SetFloat("_Highlighted", 0f);
    }

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

        gameObject.SetActive(false);
    }
}
