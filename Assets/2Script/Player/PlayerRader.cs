using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerRader : MonoBehaviour
{
    private CircleCollider2D circleCollider;

    [SerializeField] private Transform playerAim;
    private List<Transform> enemys;
    private Transform target;
    private float range = 0;

    private void Awake()
    {
        circleCollider = GetComponent<CircleCollider2D>();
        enemys = new List<Transform>();
    }

    public Transform GetTarget()
    {
        if (target == null) FindTarget();

        return target;
    }

    public void SetRange(float p_range)
    {
        circleCollider.radius = p_range;
        range = p_range;
    }

    private void FindTarget()
    {
        int t_mask = 1 << (int)ELayer.MAP | 1 << (int)ELayer.ENEMY;

        if (enemys.Count <= 0) return;

        Vector3 t_dir; Ray2D t_ray; RaycastHit2D t_hitInfo;

        for (int i = 0; i < enemys.Count; i++)
        {
            t_dir = enemys[i].position - playerAim.position;
            t_ray = new Ray2D(playerAim.position, t_dir);
            t_hitInfo = Physics2D.Raycast(t_ray.origin, t_ray.direction, range, t_mask);

            if (t_hitInfo.collider.gameObject.layer != (int)ELayer.ENEMY) continue;

            if (target == null)
            {
                target = enemys[i];
                continue;
            }

            if ((target.position - playerAim.position).sqrMagnitude > (enemys[i].position - playerAim.position).sqrMagnitude)
            {
                target = enemys[i];
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Enemy"))
        {
            enemys.Add(collision.transform);

            if (target == null)
                FindTarget();
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Enemy"))
        {
            enemys.Remove(collision.transform);

            if (collision.transform == target)
            {
                target = null;
                FindTarget();
            }
        }
    }
}
