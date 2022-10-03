using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    [SerializeField] private float speed = 0f;
    private Vector3 direction = Vector3.zero;

    [SerializeField] private ELayer targetLayer = ELayer.END;

    private int rayMask = 0;
    private RaycastHit2D hitInfo;

    [SerializeField] private GameObject hitEffect = null;

    private void Awake()
    {
        rayMask = 1 << (int)ELayer.MAP | 1 << (int)targetLayer;
    }

    public void InitValue(Vector3 p_pos, Vector3 p_dir)
    {
        transform.position = p_pos;
        transform.rotation = Quaternion.AngleAxis(Mathf.Atan2(p_dir.y, p_dir.x) * Mathf.Rad2Deg, Vector3.forward);
        direction = p_dir.normalized;
        
        StartCoroutine(CheckOnHit());
    }

    private IEnumerator CheckOnHit()
    {
        var t_deltaSpeed = speed * Time.deltaTime;

        hitInfo = Physics2D.Raycast(transform.position, direction, t_deltaSpeed, rayMask);

        while (!hitInfo)
        {
            transform.position += direction * t_deltaSpeed;
            hitInfo = Physics2D.Raycast(transform.position, direction, t_deltaSpeed, rayMask);
            yield return null;
        }

        var t_normal = hitInfo.normal;
        var t_angle = Mathf.Atan2(t_normal.y, t_normal.x) * Mathf.Rad2Deg;
        Instantiate(hitEffect, hitInfo.point, Quaternion.AngleAxis(t_angle, Vector3.forward));
        gameObject.SetActive(false);
    }
}


