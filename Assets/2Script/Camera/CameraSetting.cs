using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraSetting : MonoBehaviour
{
    [SerializeField] private float setWidth = 0;
    [SerializeField] private float setHeight = 0;
    private Camera mainCam = null;

    [SerializeField] private float size = 1f;

    private void Awake()
    {
        mainCam = Camera.main;
        mainCam.orthographicSize = size;
        SetResolution();
    }

    private void SetResolution()
    {
        var t_rect = mainCam.rect;
        var t_targetRate = setWidth / setHeight;
        var t_screenRate = (float)Screen.width / Screen.height;

        if (t_targetRate < t_screenRate) t_rect.width = t_targetRate / t_screenRate;
        else t_rect.height = t_screenRate / t_targetRate;

        t_rect.x = (1f - t_rect.width) * 0.5f; t_rect.y = (1f - t_rect.height) * 0.5f;

        mainCam.rect = t_rect;
    }
}
