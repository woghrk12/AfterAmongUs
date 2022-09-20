using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Highlighted : MonoBehaviour
{
    [SerializeField] private SpriteRenderer spriteRenderer = null;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Player")
            spriteRenderer.material.SetFloat("_Highlighted", 1f);
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.tag == "Player")
            spriteRenderer.material.SetFloat("_Highlighted", 0f);
    }
}
