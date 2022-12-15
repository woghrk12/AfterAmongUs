using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    [SerializeField] private Animator anim = null;
    [SerializeField] private SpriteRenderer sprite = null;
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
    private Coroutine attackCo = null;
    private Coroutine chaseCo = null;

    private bool IsMove { set { anim.SetBool("isWalk", value); } }
    private bool IsLeft { set { moveController.IsLeft = value; } }

    private void Start()
    {
        targetController.SetRange(chaseRange);
        hitController.StartChecking();
    }

    private void OnEnable()
    {
        hitController.HitEvent += OnHit;
        hitController.DieEvent += OnDie;
    }

    private void OnDisable()
    {
        hitController.HitEvent -= OnHit;
        hitController.DieEvent -= OnDie;
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.F1))
            StartCoroutine(Chase());
    }

    private void Move(Vector3 p_dir) => moveController.MoveCharacter(p_dir, anim);
    private IEnumerator ChaseRegion() => chaseController.ChaseRegion(inGameManager.FindRegion(curRegion));
    private IEnumerator ChasePlayer(Transform p_target) => chaseController.ChaseTarget(p_target);

    private IEnumerator Chase()
    {
        chaseCo = StartCoroutine(ChaseRegion());

        while (!targetController.Target)
        {
            Move((chaseController.Target - transform.position).normalized);
            yield return Utilities.WaitForFixedUpdate;
        }

        StopCoroutine(chaseCo);
        attackCo = StartCoroutine(Attack());
    }

    private IEnumerator Attack()
    {
        chaseCo = StartCoroutine(ChasePlayer(target));

        while (targetController.Target)
        {
            var t_target = targetController.Target;
            var t_direction = (t_target.position - enemyAim.position).normalized;
            if (Utilities.CalculateDist(enemyAim.position, t_target.position) > attackRange 
                || !attackController.CheckCanAttack(t_target, attackRange))
            {
                Move(t_direction);
                yield return Utilities.WaitForFixedUpdate;
                continue;
            }

            IsMove = false;
            IsLeft = enemyAim.position.x > t_target.position.x;
            anim.SetTrigger("Attack");
            yield return Utilities.WaitForSeconds(0.15f);
            attackController.Fire(enemyAim.position, t_direction);
            yield return Utilities.WaitForSeconds(2f);
        }

        StopCoroutine(chaseCo);
        attackCo = StartCoroutine(Chase());
    }

    private void OnHit()
    {
        StartCoroutine(HitEffect());
    }

    private IEnumerator HitEffect()
    {
        sprite.color = Color.red;
        yield return Utilities.WaitForSeconds(0.05f);
        sprite.color = Color.white;
    }

    private void OnDie()
    {
        if (!(chaseCo is null)) StopCoroutine(chaseCo);
        if (!(attackCo is null)) StopCoroutine(attackCo);
        anim.SetTrigger("Die");
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        var t_region = collision.GetComponentInParent<Region>();
        if (!t_region) return;
        curRegion = t_region;
    }
}
