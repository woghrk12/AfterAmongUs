using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyMove : MonoBehaviour
{
    private Animator anim;

    public SpriteRenderer sprite;

    [SerializeField] private PointManager pointManager;

    [SerializeField] private float moveSpeed;

    [SerializeField] private PlayerBehavior player;

    public Region region;

    private Point startPoint, targetPoint, curPoint;
    private List<Point> openList, closedList, finalList;

    private bool isChasing;
    private bool canAttack;

    private void Awake()
    {
        anim = GetComponent<Animator>();
        sprite = GetComponent<SpriteRenderer>();

        var inst = Instantiate(sprite.material);
        sprite.material = inst;

        player = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerBehavior>();

        moveSpeed = 1f;

        canAttack = true;
        isChasing = false;
    }

    private void Start()
    {
        pointManager = GameManager.Instance.pointManager;

        sprite.material.SetColor("_PlayerColor", Color.red);

        sprite.enabled = false;
        gameObject.SetActive(false);
    }

    private void Update()
    {
        if ((transform.position - player.transform.position).sqrMagnitude > 2f) return;

        if (canAttack)
            StartCoroutine(AttackCo());    
    }

    private void FixedUpdate()
    {
        if(isChasing)
            Chase();
    }

    public void SetEnemy(Region _region)
    {
        gameObject.SetActive(true);
        region = _region;
        transform.position = pointManager.GetPoint(_region.dstPoint).transform.position;
        FindPath(player.playerRegion);
        isChasing = true;
    }

    private void Chase()
    {
        if (region == player.playerRegion)
        {
            Move(player.transform);
        }
        else
        {
            if (finalList.Count == 0) return;
            
            Move(finalList[0].transform);

            if ((transform.position - finalList[0].transform.position).sqrMagnitude <= 0.01f)
                finalList.RemoveAt(0);
        }
    }

    private void Move(Transform _target)
    {
        var moveDir = (_target.position - transform.position).normalized;
        
        var curFlipX = sprite.flipX;
        sprite.flipX = (moveDir.x != 0) ? (moveDir.x < 0) : curFlipX;

        anim.SetBool("isChasing", isChasing);

        transform.position += moveDir * Time.deltaTime * moveSpeed;
    }

    private void FindPath(Region _target)
    {
        startPoint = region.FindStartPoint(transform.position);
        targetPoint = pointManager.GetPoint(_target.dstPoint);

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
                var targetCurPoint = targetPoint;

                while (targetCurPoint != startPoint)
                {
                    finalList.Add(targetCurPoint);
                    targetCurPoint = targetCurPoint.parentPoint;
                }

                finalList.Add(targetCurPoint);
                finalList.Reverse();
            }

            // add adjacent points 
            var adjList = curPoint.adj_Point;
            var adjWeight = curPoint.adj_Weight;
            for (int i = 0; i < adjList.Count; i++)
                OpenListAdd(adjList[i], adjWeight[i]);
        }
    }

    private void OpenListAdd(Point _point, float _weight)
    {
        if (closedList.Contains(_point)) return;

        if (openList.Contains(_point) && curPoint.G + _weight > _point.G) return;

        _point.SetParent(curPoint);
        _point.SetG(_weight);
        _point.SetH(_point.transform.position, targetPoint.transform.position);
        
        openList.Add(_point);
    }

    private void Attack()
    {
        
    }

    private IEnumerator AttackCo()
    {
        canAttack = false;
        isChasing = false;

        anim.SetBool("isChasing", isChasing);
        anim.SetTrigger("Attack");

        yield return new WaitForSeconds(2f);

        canAttack = true;
        isChasing = true;
    }
}
