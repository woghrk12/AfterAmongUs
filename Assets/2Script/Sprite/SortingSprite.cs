using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SortingSprite : MonoBehaviour
{
    private SpriteSorter sortingController = null;

    [SerializeField] private SpriteRenderer spriteRender = null;
    [SerializeField] private ESortingType sortingType = ESortingType.UPDATE;


    private void Start()
    {
        sortingController = GameObject.FindObjectOfType<SpriteSorter>();

        spriteRender.sortingOrder = sortingController.GetSortingOrder(transform);
    }

    private void Update()
    {
        if (sortingType == ESortingType.UPDATE)
            spriteRender.sortingOrder = sortingController.GetSortingOrder(transform);
    }
}
