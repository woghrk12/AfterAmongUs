using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterRader : MonoBehaviour
{
    [SerializeField] private CircleCollider2D circleCollider = null;

    [SerializeField] private Transform characterAim = null;
    
    private List<Transform> targets = new List<Transform>();
    private Transform target = null;

    private float range = 0f;
    [SerializeField] private string targetTag = null;
    public string TargetTag { get { return targetTag; } }

    private int rayMask = 1 << (int)ELayer.MAP | 1 << (int)ELayer.HITBOX;
    private Vector3 rayDir = Vector3.zero;
    private RaycastHit2D hitInfo;

    public void SetRange(float p_range)
    {
        range = p_range;
        circleCollider.radius = p_range;
    }

    public Transform ChaseTarget()
    {
        if (targets.Count <= 0) return null;
        if (CheckTarget(target)) return target;

        return target = FindTarget();
    }

    private bool CheckTarget(Transform p_target)
    {
        if (!p_target) return false;

        rayDir = (p_target.position - characterAim.position).normalized;
        hitInfo = Physics2D.Raycast(characterAim.position, rayDir, range, rayMask);
        
        if (!hitInfo) return false;
        return hitInfo.collider.CompareTag(targetTag);
    }

    private Transform FindTarget()
    {
        for (int i = 0; i < targets.Count; i++)
        {
            rayDir = (targets[i].position - characterAim.position).normalized;
            hitInfo = Physics2D.Raycast(characterAim.position, rayDir, range + 0.1f, rayMask);
            
            if (!hitInfo.collider.CompareTag(targetTag)) continue;
            if (!target) return targets[i];
            if ((target.position - characterAim.position).sqrMagnitude > (targets[i].position - characterAim.position).sqrMagnitude) return targets[i];
        }

        return null;
    }

    public void AddTarget(Transform p_target)
    {
        targets.Add(p_target);
        if (target) return;
        target = FindTarget();
    }

    public void RemoveTarget(Transform p_target)
    {
        targets.Remove(p_target);
        if (!p_target.Equals(target)) return;
        target = FindTarget();
    }
}
