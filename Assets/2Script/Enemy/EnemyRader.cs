using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyRader : MonoBehaviour
{
    private CircleCollider2D circleCollider;
    [SerializeField] private Transform enemyAim;
    
    private List<Transform> targets;
    private Transform target;

    private Ray2D ray;
    private Vector3 direction;
    private RaycastHit2D hitInfo;
    private float range;
    private int mask;

    private void Awake()
    {
        circleCollider = GetComponent<CircleCollider2D>();
        targets = new List<Transform>();

        mask = 1 << (int)ELayer.PLAYER | 1 << (int)ELayer.PLAYERONHIT | 1 << (int)ELayer.MAP;
    }

    private void Update()
    {
        if (target == null || !CheckTarget(target))
            FindTarget();
    }

    public Transform GetTarget()
    {
        return target;
    }

    public void SetRange(float p_range)
    {
        circleCollider.radius = p_range;
        range = p_range;
    }

    private bool CheckTarget(Transform p_target)
    {
        direction = p_target.position - enemyAim.position;
        ray = new Ray2D(enemyAim.position, direction);
        Debug.DrawRay(ray.origin, ray.direction);

        return Physics2D.Raycast(ray.origin, ray.direction, range, mask).collider.gameObject.layer != (int)ELayer.MAP;
    }

    private void FindTarget()
    {
        if (targets.Count <= 0) return;

        target = null;

        for (int i = 0; i < targets.Count; i++)
        {
            if (!CheckTarget(targets[i])) continue; 

            if (target == null)
            {
                target = targets[i];
                continue;
            }

            if ((target.position - enemyAim.position).sqrMagnitude > (targets[i].position - enemyAim.position).sqrMagnitude) 
                target = targets[i];
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        targets.Add(collision.transform);
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        targets.Remove(collision.transform);
    }
}
