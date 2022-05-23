using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Region : MonoBehaviour
{
    public int dstPoint;
    public List<Point> points;

    public Point FindStartPoint(Vector2 _position)
    {
        var minPoint = points[0];
        var minDist = DistXY(minPoint.transform.position, _position);
        for (int i = 1; i < points.Count; i++)
        {
            var temp = points[i];
            var tempDist = DistXY(temp.transform.position, _position);
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
