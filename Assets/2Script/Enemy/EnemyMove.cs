using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyMove : MonoBehaviour
{
    private Animator anim;
    private SpriteRenderer sprite;

    [SerializeField] private  PointManager pointManager;

    [SerializeField] private float moveSpeed;

    [SerializeField] private PlayerBehavior player;

    public Region region;
    [SerializeField] private Transform target;
    
    private Point startPoint, targetPoint, curPoint;
    private List<Point> openList, closedList, finalList;

    private bool isChasing;

    private void Start()
    {
        anim = GetComponent<Animator>();
        sprite = GetComponent<SpriteRenderer>();

        moveSpeed = 1f;

        pointManager = GameManager.Instance.pointManager;
        player = FindObjectOfType<PlayerBehavior>();

        isChasing = false;

        SetTarget(player.playerRegion);
    }

    private void Update()
    {
        //SetTarget(player.playerRegion);
    }

    private void FixedUpdate()
    {
        //Chase(target);
    }

    private void SetTarget(Region _target)
    {
        float distPlayer = (transform.position - player.transform.position).sqrMagnitude;

        if (distPlayer < 10f)
        {
            isChasing = true;
            target = player.transform;
        }
        else
        {
            if (isChasing)
            {
                FindPath(player.playerRegion);
                isChasing = false;
            }

            if ((transform.position - finalList[0].transform.position).sqrMagnitude < 0.01f)
                finalList.RemoveAt(0);

            target = finalList[0].transform;
        }
    }

    private void Chase(Transform _target)
    {
        Vector3 moveDir = (_target.position - transform.position).normalized;
        
        bool curFlipX = sprite.flipX;
        sprite.flipX = (moveDir.x != 0) ? (moveDir.x < 0) : curFlipX;

        anim.SetBool("isWalk", true);
        
        transform.position += moveDir * Time.deltaTime * moveSpeed;
    }

    public void FindPath(Region _target)
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
                Point targetCurPoint = targetPoint;

                while (targetCurPoint != startPoint)
                {
                    finalList.Add(targetCurPoint);
                    targetCurPoint = targetCurPoint.parentPoint;
                }

                finalList.Add(targetCurPoint);
                finalList.Reverse();
            }

            // add adjacent points 
            List<Point> adjList = curPoint.adj_Point;
            List<float> adjWeight = curPoint.adj_Weight;
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
