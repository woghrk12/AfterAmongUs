using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerRader : MonoBehaviour
{
    [SerializeField] private Transform playerAim = null;
    
    private List<Transform> enemys = new List<Transform>();
    private Transform target = null;
    public Transform Target
    {
        get
        {
            if (target == null) FindTarget();

            return target;
        }
    }
    private float range = 0f;

    private void Update()
    {
        CheckTarget(target);
    }

    public void SetRange(float p_range)
    {
        transform.localScale = new Vector3(p_range, p_range, 1f);
        range = p_range;
    }

    private void CheckTarget(Transform p_target)
    {
        if (p_target == null) return;

        int t_mask = 1 << (int)ELayer.MAP | 1 << (int)ELayer.ENEMY;
        Vector3 t_dir = p_target.position - playerAim.position;
        Ray2D t_ray = new Ray2D(playerAim.position, t_dir);
        RaycastHit2D t_hitInfo = Physics2D.Raycast(t_ray.origin, t_ray.direction, range, t_mask);

        if (t_hitInfo.collider.gameObject.layer != (int)ELayer.ENEMY)
        {
            target = null;
            FindTarget();
        }
    }

    private void FindTarget()
    {
        if (enemys.Count <= 0) return;

        int t_mask = 1 << (int)ELayer.MAP | 1 << (int)ELayer.ENEMY;

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
