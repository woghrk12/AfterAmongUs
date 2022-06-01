using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Point : MonoBehaviour
{
    public Point parentPoint;

    public List<Point> adj_Point;
    public List<float> adj_Weight;

    //public int numPoint;

    public float G, H;
    public float F { get { return G + H; } }

    private void Awake()
    {
        adj_Point = new List<Point>();
        adj_Weight = new List<float>();
    }

    private float DistXY(Vector2 p_src, Vector2 p_dst)
    {
        return (p_src - p_dst).sqrMagnitude;
    }

    public void SetPoint(List<Point> p_adj_Point)
    {
        G = 0; H = 0;
        adj_Point = p_adj_Point;

        for (int i = 0; i < adj_Point.Count; i++)
        {
            Transform t_transform = adj_Point[i].transform;
            adj_Weight.Add(DistXY(transform.position, t_transform.position));
        }
    }

    public void SetG(float p_weight)
    {
        G = parentPoint.G + p_weight;
    }

    public void SetH(Vector2 p_curPoint, Vector2 p_targetPoint)
    {
        H = DistXY(p_curPoint, p_targetPoint);
    }

    public void SetParent(Point p_parent)
    {
        parentPoint = p_parent;
    }
}
