using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyRader : MonoBehaviour
{
    private CircleCollider2D circleCollider;
    [SerializeField] private Transform enemyAim;
    private List<Transform> targets;
    private Transform target;

    private void Awake()
    {
        
        circleCollider = GetComponent<CircleCollider2D>();
        targets = new List<Transform>();
    }

    public Transform GetTarget()
    {
        return target;
    }

    public void SetRange(float p_range)
    {
        circleCollider.radius = p_range;
    }

    private void FindTarget()
    {
        if (targets.Count <= 0) return;

        for (int i = 0; i < targets.Count; i++)
        {
            if (target == null)
            {
                target = targets[i];
                continue;
            }

            if ((target.position - enemyAim.position).sqrMagnitude > (targets[i].position - enemyAim.position).sqrMagnitude) target = targets[i];
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        targets.Add(collision.transform);

        if (target == null)
        {
            FindTarget();
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        targets.Remove(collision.transform);

        if (collision.transform == target)
        {
            target = null;
            FindTarget();
        }
    }
}
