using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HighlightedObject : MonoBehaviour
{
    private SpriteRenderer spriteRenderer;

    private void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

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
