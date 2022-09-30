using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterRader : MonoBehaviour
{
    [SerializeField] private CircleCollider2D circleCollider = null;
    [SerializeField] private Transform characterAim = null;
    
    private List<Transform> targets = new List<Transform>();
    private Transform target = null;
    public Transform Target { get { return target; } }

    private float range = 0f;
    [SerializeField] private string targetTag = null;
    [SerializeField] private ELayer targetLayer = ELayer.END;
    private int rayMask = 0;
    private Vector3 rayDir = Vector3.zero;
    private RaycastHit2D hitInfo;

    private void Awake()
    {
        rayMask = 1 << (int)ELayer.MAP | 1 << (int)targetLayer;
    }

    private void Update()
    {
        if (targets.Count <= 0) return;
        CheckTarget();
    }

    public void SetRange(float p_range)
    {
        range = p_range;
        circleCollider.radius = p_range;
    }

    private void CheckTarget()
    {
        if (!target)
        {
            target = SelectTarget();
            return;
        }

        rayDir = (target.position - characterAim.position).normalized;
        hitInfo = Physics2D.Raycast(characterAim.position, rayDir, range + 0.1f, rayMask);

        if (!hitInfo)
        {
            target = SelectTarget();
            return;
        }
        if (!hitInfo.transform.Equals(target))
        {
            target = SelectTarget();
            return;
        }
    }

    private Transform SelectTarget()
    {
        var t_nearIndex = -1;
        var t_nearDist = Mathf.Infinity;
        var t_dist = 0f;

        for (int i = 0; i < targets.Count; i++)
        {
            rayDir = (targets[i].position - characterAim.position).normalized;
            hitInfo = Physics2D.Raycast(characterAim.position, rayDir, range + 0.1f, rayMask);

            if (!hitInfo.collider.CompareTag(targetTag)) continue;

            t_dist = (targets[i].position - characterAim.position).sqrMagnitude;
            if (t_nearDist > t_dist)
            {
                t_nearDist = t_dist;
                t_nearIndex = i;
            }
        }
        if (t_nearIndex < 0) return null;
        return targets[t_nearIndex];
    }

    public void AddTarget(Transform p_target)
    {
        targets.Add(p_target);
        if (target) return;
        target = SelectTarget();
    }

    public void RemoveTarget(Transform p_target)
    {
        targets.Remove(p_target);
        if (targets.Count <= 0)
        {
            target = null;
            return;
        }
        if (!p_target.Equals(target)) return;
        target = SelectTarget();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!collision.CompareTag(targetTag)) return;
        AddTarget(collision.transform);
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (!collision.CompareTag(targetTag)) return;
        RemoveTarget(collision.transform);
    }
}
