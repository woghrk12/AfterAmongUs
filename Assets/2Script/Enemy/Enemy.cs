using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    [SerializeField] private Animator anim = null;
    [SerializeField] private InGameManager inGameManager = null;

    [SerializeField] private Damagable hitController = null;
    [SerializeField] private Chasing chaseController = null;
    [SerializeField] private CharacterMove moveController = null;
    [SerializeField] private CharacterRader targetController = null;

    private float chaseRange { get { return chaseController.ChaseRange; } }
    private float attackRange = 1f;

    private Region curRegion = null;
    [SerializeField]private Transform gamePlayer = null;
    private Vector3 target { get { return chaseController.Target; } }

    private bool isMove { set { anim.SetBool("isWalk", value); } }

    private void Start()
    {
        targetController.SetRange(2f);
    }

    private IEnumerator Chase()
    {
        StartCoroutine(ChaseRegion());

        while (!targetController.Target)
        {
            Move();
            yield return null;
        }

        chaseController.IsChasing = false;

        var t_target = targetController.Target;
        StartCoroutine(ChasePlayer(t_target));

        while (Utilities.CalculateDist(transform.position, t_target.position) > attackRange || !targetController.Target)
        {
            Move();
            yield return null;
        }
        isMove = false;
        chaseController.IsChasing = false;
    }

    private void Move() => moveController.MoveCharacter((target - transform.position).normalized, anim);

    private IEnumerator ChaseRegion() => chaseController.ChaseRegion(inGameManager.FindRegion(curRegion));

    private IEnumerator ChasePlayer(Transform p_target) => chaseController.ChaseTarget(p_target);

    private void OnTriggerEnter2D(Collider2D collision)
    {
        var t_region = collision.GetComponentInParent<Region>();
        if (!t_region) return;
        curRegion = t_region;
    }
}
