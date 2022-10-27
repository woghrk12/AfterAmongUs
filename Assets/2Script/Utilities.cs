using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Utilities
{
    public static float CalculateDist(Vector3 p_from, Vector3 p_to)
    {
        var t_dx = Mathf.Abs(p_from.x - p_to.x);
        var t_dy = Mathf.Abs(p_from.y - p_to.y);

        return Mathf.Abs(t_dx - t_dy) + Mathf.Min(t_dx, t_dy) * 1.4f;
    }

    private static readonly Dictionary<float, WaitForSeconds> waitForSeconds = new Dictionary<float, WaitForSeconds>();
    public static WaitForSeconds WaitForSeconds(float p_sec)
    {
        if (waitForSeconds.ContainsKey(p_sec)) return waitForSeconds[p_sec];
        WaitForSeconds t_wfs;
        waitForSeconds.Add(p_sec, t_wfs = new WaitForSeconds(p_sec));
        return t_wfs;
    }
}
