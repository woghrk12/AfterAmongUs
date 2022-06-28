using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyNormal : EnemyBehaviour
{
    public Transform target;
    private Region attackRegion;

    [SerializeField] private EnemyRader enemyRader;

    private Point startPoint, targetPoint, curPoint;
    private List<Point> openList, closedList, finalList;

    private bool isChasing;
    private bool canAttack;

    [SerializeField] private float moveSpeed;

    [SerializeField] private Transform firePosition;
    [SerializeField] private float detectionRange;
    [SerializeField] private float fireRange;
    [SerializeField] private float fireDelay;
    private Vector3 fireDirection;

    private void Awake()
    {
        anim = GetComponent<Animator>();
        spriteRenderer = GetComponentInChildren<SpriteRenderer>();

        var t_inst = Instantiate(spriteRenderer.material);
        spriteRenderer.material = t_inst;

        spriteRenderer.material.SetColor("_PlayerColor", Color.red);

        target = null;
    }

    private void OnEnable()
    {
        attackRegion = InGameManager.instance.GetPlayerRegion();
    }

    protected override void Update()
    {
        base.Update();

        target = enemyRader.GetTarget();
        if (target == null) return;
        if ((transform.position - target.position).sqrMagnitude > fireRange * fireRange) return;
        if (canAttack) attackCo = StartCoroutine(AttackCo());
    }

    private void FixedUpdate()
    {
        if (isChasing) Chase();
    }

    public override void SetEnemy(Region p_region)
    {
        base.SetEnemy(p_region);

        enemyRader.SetRange(detectionRange);
        
        FindPath(attackRegion);
        
        isChasing = true;
        canAttack = true;
        isDie = false;
    }

    private void Chase()
    {
        if (target != null) Move(target);
        else
        {
            if (finalList.Count == 0) return;

            Move(finalList[0].transform);

            if ((transform.position - finalList[0].transform.position).sqrMagnitude <= 0.01f)
                finalList.RemoveAt(0);
        }
    }

    private void Move(Transform p_target)
    {
        var t_moveDir = (p_target.position - transform.position).normalized;

        var t_flipX = spriteRenderer.flipX;
        spriteRenderer.flipX = (t_moveDir.x != 0) ? (t_moveDir.x < 0) : t_flipX;

        anim.SetBool("isChasing", isChasing);

        transform.position += t_moveDir * Time.deltaTime * moveSpeed;
    }

    private void FindPath(Region p_target)
    {
        startPoint = spawnRegion.FindStartPoint(transform.position);
        targetPoint = PointManager.GetPoint(p_target.dstPoint);

        openList = new List<Point>() { startPoint };
        closedList = new List<Point>();
        finalList = new List<Point>();

        while (openList.Count > 0)
        {
            // Find the point with smallest cost 
            curPoint = openList[0];
            for (int i = 1; i < openList.Count; i++)
                if (openList[i].F <= curPoint.F && openList[i].H < curPoint.H)
                    curPoint = openList[i];

            openList.Remove(curPoint);
            closedList.Add(curPoint);

            // reach the final destination
            if (curPoint == targetPoint)
            {
                var t_curPoint = targetPoint;

                while (t_curPoint != startPoint)
                {
                    finalList.Add(t_curPoint);
                    t_curPoint = t_curPoint.parentPoint;
                }

                finalList.Add(t_curPoint);
                finalList.Reverse();
            }

            // add adjacent points 
            var t_adjList = curPoint.adj_Point;
            var t_adjWeight = curPoint.adj_Weight;
            for (int i = 0; i < t_adjList.Count; i++)
                OpenListAdd(t_adjList[i], t_adjWeight[i]);
        }
    }

    private void OpenListAdd(Point p_point, float p_weight)
    {
        if (closedList.Contains(p_point)) return;

        if (openList.Contains(p_point) && curPoint.G + p_weight > p_point.G) return;

        p_point.SetParent(curPoint);
        p_point.SetG(p_weight);
        p_point.SetH(p_point.transform.position, targetPoint.transform.position);

        openList.Add(p_point);
    }

    private Vector3 Targeting()
    {
        var t_direction = (target.position - firePosition.position).normalized;
        var t_angle = Mathf.Atan2(t_direction.y, t_direction.x) * Mathf.Rad2Deg;
        var t_rotation = Quaternion.AngleAxis(t_angle, Vector3.forward);

        firePosition.rotation = t_rotation;

        return t_direction;
    }

    private IEnumerator AttackCo()
    {
        canAttack = false;
        isChasing = false;

        anim.SetBool("isChasing", isChasing);
        anim.SetTrigger("Attack");

        yield return new WaitForSeconds(0.1f);

        fireDirection = Targeting();
        var t_bullet = ObjectPooling.SpawnObject("Enemy Bullet", firePosition.position, firePosition.rotation).GetComponent<Bullet>();
        t_bullet.SetDirection(fireDirection);

        yield return new WaitForSeconds(fireDelay);

        canAttack = true;
        isChasing = true;
    }

    protected override IEnumerator DieCo()
    {
        canAttack = false;
        isChasing = false;

        return base.DieCo();
    }
}
