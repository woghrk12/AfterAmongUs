using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    [SerializeField] private Transform target = null;
    [SerializeField] private Vector3 offset = Vector3.zero;
    [SerializeField] private float followSpeed = 0f;

    [SerializeField] private Vector2 minPos = Vector2.zero;
    [SerializeField] private Vector2 maxPos = Vector2.zero;

    private void LateUpdate()
    {
        var t_desiredPos = target.position + offset;
        var t_rawPos = Vector3.Lerp(transform.position, t_desiredPos, followSpeed * Time.deltaTime);
        transform.position = LimitArea(t_rawPos);
    }

    private Vector3 LimitArea(Vector3 p_cameraPos)
    {
        Vector3 t_pos = p_cameraPos;

        if (t_pos.x < minPos.x) t_pos.x = minPos.x;
        if (t_pos.x > maxPos.x) t_pos.x = maxPos.x;
        if (t_pos.y < minPos.y) t_pos.y = minPos.y;
        if (t_pos.y > maxPos.y) t_pos.y = maxPos.y;

        return t_pos;
    }
}
