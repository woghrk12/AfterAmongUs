using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMove : MonoBehaviour
{
    [SerializeField] private int width, height;
    [SerializeField] private float limitMinX, limitMaxX, limitMinY, limitMaxY;
    
    private Transform player;

    private void Awake()
    {
        Camera camera = GetComponent<Camera>();
        Rect rect = camera.rect;
        float scaleHeight = ((float)Screen.width / Screen.height) / ((float)width / height);
        float scaleWidth = 1f / scaleHeight;

        if (scaleHeight < 1)
        {
            rect.height = scaleHeight;
            rect.y = (1f - scaleHeight) / 2f;
        }
        else
        {
            rect.width = scaleWidth;
            rect.x = (1f - scaleWidth) / 2f;

        }

        camera.rect = rect;

        player = GameObject.FindGameObjectWithTag("Player").transform;
    }

    private void LateUpdate()
    {
        Follow(player);
    }

    private void Follow(Transform target)
    {
        float x = Mathf.Clamp(target.position.x, limitMinX, limitMaxX);
        float y = Mathf.Clamp(target.position.y, limitMinY, limitMaxY);
        transform.position = new Vector3(x, y, -10);
    }
}
