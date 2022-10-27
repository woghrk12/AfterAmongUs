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

    private float chaseRange { get { return chaseController.ChaseRange; } }
    private float attackRange = 0f;

    private Region curRegion = null;
    [SerializeField]private Transform gamePlayer = null;
    private Vector3 target { get { return chaseController.Target; } }

    private IEnumerator Chase()
    {
        StartCoroutine(ChaseRegion());

        while (Utilities.CalculateDist(transform.position, gamePlayer.position) > chaseRange)
        {
            Move();
            yield return null;
        }

        chaseController.IsChasing = false;

        StartCoroutine(ChasePlayer());

        while (Utilities.CalculateDist(transform.position, gamePlayer.position) > attackRange)
        {
            Move();
            yield return null;
        }

        chaseController.IsChasing = false;
    }

    private void Move() => moveController.MoveCharacter((target - transform.position).normalized, anim);

    private IEnumerator ChaseRegion() => chaseController.ChaseRegion(inGameManager.FindRegion(curRegion));

    private IEnumerator ChasePlayer() => chaseController.ChaseTarget(gamePlayer);

    private void OnTriggerEnter2D(Collider2D collision)
    {
        var t_region = collision.GetComponentInParent<Region>();
        if (!t_region) return;
        curRegion = t_region;
    }
}
