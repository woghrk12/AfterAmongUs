using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PointManager : MonoBehaviour
{
    public static PointManager instance;
    [SerializeField] private List<Point> pointLists;

    private void Awake()
    {
        instance = this;
    }

    private void Start()
    {
        int[][] arr =
        {
            new int[] { 1 },                    // 0
            new int[] { 0, 61 },                // 1 
            new int[] { 3, 5, 8 },              // 2
            new int[] { 2, 4, 7 },              // 3
            new int[] { 3, 5 },                 // 4
            new int[] { 2, 4, 6 },              // 5
            new int[] { 5, 47 },                // 6
            new int[] { 3, 60 },                // 7
            new int[] { 2, 61 },                // 8
            new int[] { 10 },                   // 9
            new int[] { 9, 55 },                // 10
            new int[] { 12 },                   // 11
            new int[] { 11, 13 },               // 12   
            new int[] { 12, 14 },               // 13
            new int[] { 13, 15 },               // 14
            new int[] { 14, 16 },               // 15 
            new int[] { 15, 17 },               // 16
            new int[] { 16, 56 },               // 17
            new int[] { 19, 20 },               // 18
            new int[] { 18, 60 },               // 19
            new int[] { 18, 59 },               // 20
            new int[] { 22, 58 },               // 21
            new int[] { 21, 23 },               // 22
            new int[] { 22, 59 },               // 23
            new int[] { 25 },                   // 24
            new int[] { 24, 26 },               // 25
            new int[] { 25, 60 },               // 26
            new int[] { 28 },                   // 27
            new int[] { 27, 52 },               // 28
            new int[] { 30 },                   // 29
            new int[] { 29, 50 },               // 30
            new int[] { 32 },                   // 31
            new int[] { 31, 59 },               // 32
            new int[] { 34 },                   // 33
            new int[] { 33, 59 },               // 34
            new int[] { 36, 37, 43 },           // 35   
            new int[] { 35, 61 },               // 36
            new int[] { 35, 38 },               // 37
            new int[] { 37, 39, 55 },           // 38
            new int[] { 38, 40 },               // 39
            new int[] { 39, 41 },               // 40
            new int[] { 40, 42, 56 },           // 41
            new int[] { 41, 43 },               // 42
            new int[] { 35, 42 },               // 43
            new int[] { 45, 55 },               // 44
            new int[] { 44, 46 },               // 45
            new int[] { 45, 54 },               // 46
            new int[] { 6, 48 },                // 47
            new int[] { 47, 49 },               // 48
            new int[] { 48, 50 },               // 49
            new int[] { 30, 49, 51 },           // 50
            new int[] { 50, 52 },               // 51
            new int[] { 28, 51, 53 },           // 52
            new int[] { 52, 54 },               // 53
            new int[] { 46, 53 },               // 54
            new int[] { 10, 38, 44 },           // 55
            new int[] { 17, 41, 57 },           // 56
            new int[] { 56, 58 },               // 57
            new int[] { 21, 57 },               // 58
            new int[] { 20, 23, 32, 34 },       // 59
            new int[] { 7, 19, 26 },            // 60
            new int[] { 1, 8, 36 }              // 61
        };

        for (int i = 0; i < pointLists.Count; i++)
        {
            var temp = new List<Point>();
            var arr_temp = arr[i];

            for (int j = 0; j < arr_temp.Length; j++)
                temp.Add(pointLists[arr_temp[j]]);

            pointLists[i].GetComponent<Point>().SetPoint(temp);
        }
    }

    public Point GetPoint(int num)
    {
        return pointLists[num];
    }
}
