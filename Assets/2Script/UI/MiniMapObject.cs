using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MiniMapObject : MonoBehaviour
{
    private RectTransform rect;
    private Image image;

    private void Awake()
    {
        rect = GetComponent<RectTransform>();
        image = GetComponent<Image>();
    }

    public void SetObject(Vector2 p_position, Sprite p_sprite)
    {
        rect.anchoredPosition = p_position;
        image.sprite = p_sprite;
    }
}
