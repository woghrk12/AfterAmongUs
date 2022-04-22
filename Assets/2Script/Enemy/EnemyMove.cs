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
    private List<Point> openList, closedList;//, finalList;
    [SerializeField] List<Point> finalList;

    public bool isChasing;

    private void Awake()
    {
        anim = GetComponent<Animator>();
        sprite = GetComponent<SpriteRenderer>();

        var inst = Instantiate(sprite.material);
        sprite.material = inst;

        isChasing = false;
    }

    private void Start()
    {
        sprite.material.SetColor("_PlayerColor", Color.red);

        moveSpeed = 1f;

        pointManager = GameManager.Instance.pointManager;
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerBehavior>();

        gameObject.SetActive(false);
    }

    private void FixedUpdate()
    {
        Chase();
    }

    private void Chase()
    {
        if (isChasing)
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

        anim.SetBool("isWalk", true);
        
        transform.position += moveDir * Time.deltaTime * moveSpeed;
    }

    public void FindRegion() => FindPath(player.playerRegion);

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
}
