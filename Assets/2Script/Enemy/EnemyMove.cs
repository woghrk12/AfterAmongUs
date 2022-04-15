using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyMove : MonoBehaviour
{
    private Animator anim;
    private SpriteRenderer sprite;

    [SerializeField] private  PointManager pointManager;

    [SerializeField] private float moveSpeed;

    [SerializeField] private Transform player;

    public Region region;
    [SerializeField] private Transform target;
    
    private Point startPoint, targetPoint, curPoint;
    private List<Point> openList, closedList, finalList;

    private void Start()
    {
        anim = GetComponent<Animator>();
        sprite = GetComponent<SpriteRenderer>();

        pointManager = GameManager.Instance.pointManager;
        player = FindObjectOfType<PlayerBehavior>().transform;
    }

    private void FixedUpdate()
    {
        //Chase(target);    
    }

    public void SetTarget(Transform _target)
    {
        target = _target;
    }

    private void Chase(Transform _target)
    {
        Vector3 moveDir = Vector3.ClampMagnitude((_target.position - transform.position), 1f);
        
        bool curFlipX = sprite.flipX;
        sprite.flipX = (moveDir.x != 0) ? (moveDir.x < 0) : curFlipX;

        anim.SetBool("isWalk", true);
        
        transform.position += moveDir * Time.deltaTime * moveSpeed;
    }

    private void FindPath(Region _target)
    {
        startPoint = pointManager.GetPoint(region.dstPoint);
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
            for (int i = 0; i < adjList.Count; i++)
                OpenListAdd(adjList[i]);
        }
    }

    private void OpenListAdd(Point _point)
    {
        if (!closedList.Contains(_point))
        { 
        
        }
    }
}
