using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    [SerializeField] private Transform target;

    [SerializeField] private float limitMinX, limitMaxX, limitMinY, limitMaxY;

    private void LateUpdate()
    {
        Follow(target);
    }

    private void Follow(Transform p_transform)
    {
        float t_x = Mathf.Clamp(p_transform.position.x, limitMinX, limitMaxX);
        float t_y = Mathf.Clamp(p_transform.position.y, limitMinY, limitMaxY);
        transform.position = new Vector3(t_x, t_y, -10);
    }
}
