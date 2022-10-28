using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    [SerializeField] private Animator anim = null;
    [SerializeField] private InGameManager inGameManager = null;

    [SerializeField] private Damagable hitController = null;
    [SerializeField] private Chasing chaseController = null;
    [SerializeField] private Attack attackController = null;
    [SerializeField] private CharacterMove moveController = null;
    [SerializeField] private CharacterRader targetController = null;

    private Region curRegion = null;
    private Transform target { get { return targetController.Target; } }

    [SerializeField] private float chaseRange = 0f;
    [SerializeField] private float attackRange = 0f;
    [SerializeField] private Transform enemyAim = null;
    private bool IsMove { set { anim.SetBool("isWalk", value); } }
    private bool IsLeft { set { moveController.IsLeft = value; } }

    private void Start()
    {
        targetController.SetRange(chaseRange);
    }

    private IEnumerator Chase()
    {
        StartCoroutine(ChaseRegion());

        while (!targetController.Target)
        {
            Move((chaseController.Target - transform.position).normalized);
            yield return Utilities.WaitForFixedUpdate;
        }

        chaseController.IsChasing = false;
    }

    private void Move(Vector3 p_dir) => moveController.MoveCharacter(p_dir, anim);

    private IEnumerator ChaseRegion() => chaseController.ChaseRegion(inGameManager.FindRegion(curRegion));
    private IEnumerator ChasePlayer(Transform p_target) => chaseController.ChaseTarget(p_target);

    private IEnumerator Attack()
    {
        StartCoroutine(ChasePlayer(target));
        while (targetController.Target)
        {
            while (Utilities.CalculateDist(transform.position, target.position) > attackRange)
            {
                Move((target.position - transform.position).normalized);
                yield return Utilities.WaitForFixedUpdate;
            }

            IsMove = false;

            IsLeft = enemyAim.position.x > target.position.x;
            anim.SetTrigger("Attack");
            yield return Utilities.WaitForSeconds(0.15f);
            attackController.Fire(enemyAim.position, (target.position - enemyAim.position).normalized);
            yield return Utilities.WaitForSeconds(2f);
        }
        chaseController.IsChasing = false;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        var t_region = collision.GetComponentInParent<Region>();
        if (!t_region) return;
        curRegion = t_region;
    }
}
