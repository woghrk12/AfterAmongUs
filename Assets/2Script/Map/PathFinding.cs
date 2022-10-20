using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Node
{
    public Node(bool p_isWall, int p_xIdx, int p_yIdx, float p_xPos, float p_yPos) 
    { 
        isWall = p_isWall;
        xIdx = p_xIdx; yIdx = p_yIdx;
        xPos = p_xPos; yPos = p_yPos; 
    }

    public bool isWall;
    public Node parentNode;

    public int xIdx, yIdx;
    public float xPos, yPos;
    public int g, h;
    public int f { get { return g + h; } }
}

public class PathFinding : MonoBehaviour
{
    [SerializeField] private Vector2Int bottomLeft, topRight;
    [SerializeField] private Vector2Int startPos, targetPos;

    [SerializeField] private bool allowDiagonal, dontCrossCorner;

    private int sizeX, sizeY;
    private Node[,] nodeArray;

    private Node startNode, targetNode, curNode;

    private List<Node> openList, closedList, finalNodeList;

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.P))
            FindPath(startPos, targetPos);
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

    public void FindPath(Vector2Int p_startPos, Vector2Int p_targetPos)
    {
        startNode = nodeArray[p_startPos.x - bottomLeft.x, p_startPos.y - bottomLeft.y];
        targetNode = nodeArray[p_targetPos.x - bottomLeft.x, p_targetPos.y - bottomLeft.y];

        openList = new List<Node> { startNode };
        closedList = new List<Node>();
        finalNodeList = new List<Node>();

        while (openList.Count > 0)
        {
            curNode = openList[0];
            for (int i = 1; i < openList.Count; i++)
                if (openList[i].f <= curNode.f && openList[i].h < curNode.h) curNode = openList[i];

            openList.Remove(curNode);
            closedList.Add(curNode);

            if (curNode.Equals(targetNode))
            {
                var t_curNode = curNode;
                
                while (!t_curNode.Equals(startNode))
                {
                    finalNodeList.Add(t_curNode);
                    t_curNode = t_curNode.parentNode;
                }

                finalNodeList.Add(startNode);
                finalNodeList.Reverse();

                return;
            }

            if (allowDiagonal)
            {
                OpenListAdd(curNode, targetNode, curNode.xIdx + 1, curNode.yIdx + 1);
                OpenListAdd(curNode, targetNode, curNode.xIdx - 1, curNode.yIdx + 1);
                OpenListAdd(curNode, targetNode, curNode.xIdx - 1, curNode.yIdx - 1);
                OpenListAdd(curNode, targetNode, curNode.xIdx + 1, curNode.yIdx - 1);
            }
            OpenListAdd(curNode, targetNode, curNode.xIdx, curNode.yIdx + 1);
            OpenListAdd(curNode, targetNode, curNode.xIdx + 1, curNode.yIdx);
            OpenListAdd(curNode, targetNode, curNode.xIdx, curNode.yIdx - 1);
            OpenListAdd(curNode, targetNode, curNode.xIdx - 1, curNode.yIdx);
        }
    }

    private void OpenListAdd(Node p_curNode, Node p_targetNode, int p_checkX, int p_checkY)
    {
        if (p_checkX < 0 || p_checkY < 0 || p_checkX >= sizeX || p_checkY >= sizeY) return;

        var t_node = nodeArray[p_checkX, p_checkY];

        if (t_node.isWall) return;
        if (closedList.Contains(t_node)) return;
        if (allowDiagonal && (nodeArray[p_curNode.xIdx, p_checkY].isWall && nodeArray[p_checkX, p_curNode.yIdx].isWall)) return;
        if (dontCrossCorner && (nodeArray[p_curNode.xIdx, p_checkY].isWall || nodeArray[p_checkX, p_curNode.yIdx].isWall)) return;

        int t_moveCost = p_curNode.g + (p_curNode.xIdx.Equals(p_checkX) || p_curNode.yIdx.Equals(p_checkY) ? 10 : 14);

        if (openList.Contains(t_node) && t_moveCost >= t_node.g) return;
        
        t_node.g = t_moveCost;
        t_node.h = (Mathf.Abs(p_checkX - p_targetNode.xIdx) + Mathf.Abs(p_checkY - p_targetNode.yIdx)) * 10;
        t_node.parentNode = p_curNode;

        openList.Add(t_node);
    }
}
