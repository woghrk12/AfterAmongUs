using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Region : MonoBehaviour
{
    [SerializeField] private Vector2Int bottomLeft = Vector2Int.zero;
    [SerializeField] private Vector2Int topRight = Vector2Int.zero;
    [SerializeField] private Vector2Int targetPos = Vector2Int.zero;

    [SerializeField] private Region[] adjRegion = null;
    [SerializeField] private Vector2Int[] adjPoint = null;

    private int sizeX = 0;
    private int sizeY = 0;
    private Node[,] nodeArray = null;

    public Vector2Int TargetPos { get { return targetPos; } }
    public Node[,] NodeArray { get { return nodeArray; } }

    private void Start()
    {
        CheckWall();
    }

    private void CheckWall()
    {
        sizeX = topRight.x - bottomLeft.x + 1;
        sizeY = topRight.y - bottomLeft.y + 1;

        nodeArray = new Node[sizeX, sizeY];

        for (int i = 0; i < sizeX; i++)
        {
            for (int j = 0; j < sizeY; j++)
            {
                bool isWall = false;
                if (Physics2D.OverlapCircle(new Vector2((i + bottomLeft.x) * 0.1f, (j + bottomLeft.y) * 0.1f), 0.1f, 1 << (int)ELayer.MAP))
                    isWall = true;

                nodeArray[i, j] = new Node(isWall, i, j, (i + bottomLeft.x) * .1f, (j + bottomLeft.y) * .1f);
            }
        }
    }

    public Vector2Int GetAdjPoint(Region p_region)
    {
        for (int i = 0; i < adjRegion.Length; i++)
        {
            if (adjRegion[i].Equals(p_region)) return adjPoint[i];
        }

        return Vector2Int.zero;
    }
}
