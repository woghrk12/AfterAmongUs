using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Chasing : MonoBehaviour
{
    [SerializeField] private InGameManager inGameManager = null;

    [SerializeField] private Region curRegion = null;
    [SerializeField] private Transform gamePlayer = null;

    [SerializeField] private float chaseRange = 0f;
    [SerializeField] private float speed = 0f;

    private Vector3 target = Vector3.zero;
    private float targetDist = 0f;

    private Coroutine chaseCo = null;

    private bool isPlayerInRange { get { return targetDist < chaseRange; } }

    private void Start()
    {
        chaseCo = StartCoroutine(ChaseRegion());
    }

    private void Update()
    {
        targetDist = CalculateDistance(gamePlayer.position);
    }

    private void FixedUpdate()
    {
        
    }

    private IEnumerator Chase(Vector3 p_target, float p_attackRange)
    {
        //StartCoroutine(MoveCo(target));

        //
        chaseCo = StartCoroutine(ChaseRegion());
        yield return new WaitUntil(() => !isPlayerInRange);
        StopCoroutine(chaseCo);

        target = p_target;
        
        //
    }

    private IEnumerator ChaseRegion()
    {
        var t_regionList = inGameManager.FindRegion(curRegion);
        var t_curRegion = t_regionList[0];
        var t_curPos = new Vector2Int((int)(transform.position.x * 10f), (int)(transform.position.y * 10f));

        while (t_regionList.Count > 0)
        {
            var t_targetPos = t_regionList.Count > 1 ? t_curRegion.GetAdjPoint(t_regionList[1]) : t_curRegion.TargetPos;

            var t_nodeList = t_curRegion.FindPath(t_curPos, t_targetPos);

            while (t_nodeList.Count > 0)
            {
                target = t_nodeList[0].Position;
                yield return new WaitUntil(() => (transform.position - t_nodeList[0].Position).sqrMagnitude <= 0.001f);
                t_nodeList.RemoveAt(0);
            }

            t_curPos = t_targetPos;

            t_regionList.RemoveAt(0);
            if (t_regionList.Count > 0) t_curRegion = t_regionList[0];
        }
    }


    private float CalculateDistance(Vector3 p_target)
    {
        var dx = transform.position.x - p_target.x;
        var dy = transform.position.y - p_target.y;

        return Mathf.Abs(dx - dy) + Mathf.Min(dx, dy) * 1.4f;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        var t_region = collision.GetComponentInParent<Region>();
        if (!t_region) return;
        curRegion = t_region;
    }
}
