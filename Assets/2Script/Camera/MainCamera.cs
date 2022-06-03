using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class MainCamera : MonoBehaviour
{
    [SerializeField] private int width, height;

    private void Awake()
    {
        Camera t_camera = GetComponent<Camera>();

        Rect t_rect = t_camera.rect;
        float t_scaleHeight = ((float)Screen.width / Screen.height) / ((float)width / height);
        float t_scaleWidth = 1f / t_scaleHeight;

        if (t_scaleHeight < 1)
        {
            t_rect.height = t_scaleHeight;
            t_rect.y = (1f - t_scaleHeight) / 2f;
        }
        else
        {
            t_rect.width = t_scaleWidth;
            t_rect.x = (1f - t_scaleWidth) / 2f;
        }

        t_camera.rect = t_rect;
    }
}
