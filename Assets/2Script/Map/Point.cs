using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Point : MonoBehaviour
{
    public Point parentPoint;

    public List<Point> adj_Point;
    public List<float> adj_Weight;

    public int numPoint;

    public float G, H;
    public float F { get { return G + H; } }

    private void Awake()
    {
        adj_Point = new List<Point>();
        adj_Weight = new List<float>();
    }
    private float DistXY(Vector2 src, Vector2 dst)
    {
        return (src - dst).sqrMagnitude;
    }

    public void SetPoint(int num, List<Point> _adj_Point)
    {
        G = 0; H = 0;
        numPoint = num;
        adj_Point = _adj_Point;

        for (int i = 0; i < adj_Point.Count; i++)
        {
            Transform temp = adj_Point[i].transform;
            adj_Weight.Add(DistXY(transform.position, temp.position));
        }
    }

    public void SetG(float _weight)
    {
        G = parentPoint.G + _weight;
    }

    public void SetH(Vector2 curPoint, Vector2 targetPoint)
    {
        H = DistXY(curPoint, targetPoint);
    }

    public void SetParent(Point _parent)
    {
        parentPoint = _parent;
    }
}
