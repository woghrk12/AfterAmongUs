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

    public Region[] AdjRegion { get { return adjRegion; } }

    private int sizeX = 0;
    private int sizeY = 0;
    private Node[,] nodeArray = null;

    private Region parentRegion = null;
    public Region ParentRegion { set { parentRegion = value; } get { return parentRegion; } }

    private float g = 0, h = 0;

    public float G { set { g = value; } get { return g; } }
    public float H { set { h = value; } get { return h; } }
    public float F { get { return g + h; } }

    public Vector2Int TargetPos { get { return targetPos; } }
    public Node[,] NodeArray { get { return nodeArray; } }

    private PathFindingByNode pathController = null;

    private void Awake()
    {
        pathController = new PathFindingByNode();
    }

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

    public List<Node> FindPath(Vector2Int p_startPos, Vector2Int p_targetPos)
    {
        var t_xStart = p_startPos.x - bottomLeft.x;
        var t_yStart = p_startPos.y - bottomLeft.y;
        var t_xTarget = p_targetPos.x - bottomLeft.x;
        var t_yTarget = p_targetPos.y - bottomLeft.y;

        if (t_xStart < 0 || t_yStart < 0 || t_xStart >= sizeX || t_yStart >= sizeY) return null;
        if (t_xTarget < 0 || t_yTarget < 0 || t_xTarget >= sizeX || t_yTarget >= sizeY) return null;
        
        return pathController.FindPath(p_startPos, p_targetPos, nodeArray, sizeX, sizeY, bottomLeft);
    }
}
