using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PointManager : MonoBehaviour
{
    [SerializeField] private List<Point> pointLists;

    private void Start()
    {
        int[][] arr =
        {
            new int[] { },
            new int[] { 41 },               // 1 
            new int[] { 2, 3, 20 },         // 2
            new int[] { 1, 4, 5 },          // 3
            new int[] { 1, 4, 41 },         // 4
            new int[] { 2, 3 },             // 5
            new int[] { 2, 6 },             // 6
            new int[] { 5, 7 },             // 7
            new int[] { 6, 8, 9 },          // 8
            new int[] { 7 },                // 9
            new int[] { 7, 10 },            // 10
            new int[] { 9, 11, 12 },        // 11
            new int[] { 10 },               // 12   
            new int[] { 10, 13 },           // 13
            new int[] { 12, 14 },           // 14
            new int[] { 13, 15 },           // 15 
            new int[] { 14, 16, 17 },       // 16
            new int[] { 15 },               // 17
            new int[] { 15, 18, 22 },       // 18
            new int[] { 17, 19 },           // 19
            new int[] { 18, 20, 24 },       // 20
            new int[] { 1, 19, 21 },        // 21
            new int[] { 20 },               // 22
            new int[] { 17, 23 },           // 23
            new int[] { 22, 25 },           // 24
            new int[] { 19, 25 },           // 25
            new int[] { 23, 24, 25 },       // 26
            new int[] { 25, 27, 30 },       // 27
            new int[] { 26, 28 },           // 28
            new int[] { 27, 29 },           // 29
            new int[] { 28 },               // 30
            new int[] { 26, 31 },           // 31
            new int[] { 30, 32 },           // 32
            new int[] { 31, 33 },           // 33
            new int[] { 32, 34 },           // 34
            new int[] { 33, 35 },           // 35   
            new int[] { 34, 36, 37, 38 },   // 36
            new int[] { 35 },               // 37
            new int[] { 35 },               // 38
            new int[] { 35, 39 },           // 39
            new int[] { 38, 40 },           // 40
            new int[] { 39, 41 },           // 41
            new int[] { 0, 3, 40 }          // 42
        };

        for (int i = 0; i < pointLists.Count; i++)
        {
            var temp = new List<Point>();
            var arr_temp = arr[i];

            for (int j = 0; j < arr_temp.Length; j++)
                temp.Add(pointLists[arr_temp[j]]);

            pointLists[i].GetComponent<Point>().SetPoint(i, temp);
        }
    }

    public Point GetPoint(int num)
    {
        return pointLists[num];
    }
}
