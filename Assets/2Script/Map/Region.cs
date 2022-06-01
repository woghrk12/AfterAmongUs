using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Region : MonoBehaviour
{
    public int dstPoint;
    public List<Point> points;

    public Point FindStartPoint(Vector2 p_position)
    {
        var t_minPoint = points[0];
        var t_minDist = DistXY(t_minPoint.transform.position, p_position);
        for (int i = 1; i < points.Count; i++)
        {
            var t_point = points[i];
            var t_dist = DistXY(t_point.transform.position, p_position);
            if (t_minDist > t_dist)
            {
                t_minPoint = t_point;
                t_minDist = t_dist;
            }
        }

        return t_minPoint;
    }

    private float DistXY(Vector2 p_src, Vector2 p_dst)
    {
        return (p_src - p_dst).sqrMagnitude;
    }
}
