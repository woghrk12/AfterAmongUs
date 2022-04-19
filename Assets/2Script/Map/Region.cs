using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Region : MonoBehaviour
{
    public int dstPoint;
    public List<int> points;
    public PointManager pointManager;

    public Point FindStartPoint(Vector2 _position)
    {
        Point minPoint = pointManager.GetPoint(points[0]);
        float minDist = DistXY(minPoint.transform.position, _position);
        for (int i = 1; i < points.Count; i++)
        {
            Point temp = pointManager.GetPoint(points[i]);
            float tempDist = DistXY(temp.transform.position, _position);
            if (minDist > tempDist)
            {
                minPoint = temp;
                minDist = tempDist;
            }
        }

        return minPoint;
    }

    private float DistXY(Vector2 src, Vector2 dst)
    {
        return (src - dst).sqrMagnitude;
    }
}
