using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    [SerializeField] private InGameManager inGameManager = null;

    [SerializeField] private Damagable hitController = null;
    [SerializeField] private Chasing chaseController = null;

    private Region curRegion = null;
    private Transform gamePlayer = null;
    private Vector3 target { get { return chaseController.Target; } }


    private IEnumerator ChaseRegion() => chaseController.ChaseRegion(inGameManager.FindRegion(curRegion));

    private IEnumerator ChasePlayer() => chaseController.ChaseTarget(gamePlayer);

    private void OnTriggerEnter2D(Collider2D collision)
    {
        var t_region = collision.GetComponentInParent<Region>();
        if (!t_region) return;
        curRegion = t_region;
    }
}
