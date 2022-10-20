using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PathFindingByNode 
{
    private int[] dx = { 0, 1, 1, 1, 0, -1, -1, -1 };
    private int[] dy = { 1, 1, 0, -1, -1, -1, 0, 1 };

    private Node startNode, targetNode, curNode;
    private List<Node> openNodeList, closedNodeList, finalNodeList;

    public List<Node> FindPath(Vector2Int p_startPos, Vector2Int p_targetPos, Node[,] p_nodeArray, int p_sizeX, int p_sizeY, Vector2Int p_bottomLeft)
    {
        FindPathByNode(p_startPos, p_targetPos, p_nodeArray, p_sizeX, p_sizeY, p_bottomLeft);
        return finalNodeList;
    }

    private void FindPathByNode(Vector2Int p_startPos, Vector2Int p_targetPos, Node[,] p_nodeArray, int p_sizeX, int p_sizeY, Vector2Int p_bottomLeft)
    {
        startNode = p_nodeArray[p_startPos.x - p_bottomLeft.x, p_startPos.y - p_bottomLeft.y];
        targetNode = p_nodeArray[p_targetPos.x - p_bottomLeft.x, p_targetPos.y - p_bottomLeft.y];

        openNodeList = new List<Node> { startNode };
        closedNodeList = new List<Node>();
        finalNodeList = new List<Node>();

        while (openNodeList.Count > 0)
        {
            curNode = openNodeList[0];
            for (int i = 1; i < openNodeList.Count; i++)
                if (openNodeList[i].f <= curNode.f && openNodeList[i].h < curNode.h) curNode = openNodeList[i];

            openNodeList.Remove(curNode);
            closedNodeList.Add(curNode);

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

            for (int i = 0; i < 8; i++)
            {
                var t_xIdx = curNode.xIdx + dx[i];
                var t_yIdx = curNode.yIdx + dy[i];
                if (t_xIdx < 0 || t_yIdx < 0 || t_xIdx >= p_sizeX || t_yIdx >= p_sizeY) continue;

                OpenNodeListAdd(p_nodeArray, curNode, targetNode, p_nodeArray[t_xIdx, t_yIdx]);
            }
        }
    }

    private void OpenNodeListAdd(Node[,] p_nodeArray, Node p_curNode, Node p_targetNode, Node p_checkNode)
    {
        if (p_checkNode.isWall) return;
        if (closedNodeList.Contains(p_checkNode)) return;
        if (p_nodeArray[p_curNode.xIdx, p_checkNode.yIdx].isWall || p_nodeArray[p_checkNode.xIdx, p_curNode.yIdx].isWall) return;

        int t_moveCost = p_curNode.g + (p_curNode.xIdx.Equals(p_checkNode.xIdx) || p_curNode.yIdx.Equals(p_checkNode.yIdx) ? 10 : 14);

        if (openNodeList.Contains(p_checkNode) && t_moveCost >= p_checkNode.g) return;

        p_checkNode.g = t_moveCost;
        p_checkNode.h = (Mathf.Abs(p_checkNode.xIdx - p_targetNode.xIdx) + Mathf.Abs(p_checkNode.yIdx - p_targetNode.yIdx)) * 10;
        p_checkNode.parentNode = p_curNode;

        openNodeList.Add(p_checkNode);
    }
}
