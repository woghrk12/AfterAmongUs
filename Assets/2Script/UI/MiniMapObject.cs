using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MiniMapObject : MonoBehaviour
{
    private RectTransform rect;

    private void Awake()
    {
        rect = GetComponent<RectTransform>();
    }

    public void SetPosition(Vector2 p_position)
    {
        rect.anchoredPosition = p_position;
    }
}
