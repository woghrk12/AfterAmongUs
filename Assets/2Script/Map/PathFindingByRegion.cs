using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PathFindingByRegion
{
    private Region startRegion, targetRegion, curRegion;
    private List<Region> openRegionList, closedRegionList, finalRegionList;

    public List<Region> FindPath(Region p_startRegion, Region p_targetRegion)
    {
        FindPathByRegion(p_startRegion, p_targetRegion);
        return finalRegionList;
    }

    private void FindPathByRegion(Region p_startRegion, Region p_targetRegion)
    {
        startRegion = p_startRegion;
        targetRegion = p_targetRegion;

        openRegionList = new List<Region>() { startRegion };
        closedRegionList = new List<Region>();
        finalRegionList = new List<Region>();

        while (openRegionList.Count > 0)
        {
            curRegion = openRegionList[0];
            for (int i = 1; i < openRegionList.Count; i++)
                if (openRegionList[i].F <= curRegion.F && openRegionList[i].H < curRegion.H) curRegion = openRegionList[i];

            openRegionList.Remove(curRegion);
            closedRegionList.Add(curRegion);

            if (curRegion.Equals(targetRegion))
            {
                var t_curRegion = targetRegion;

                while (!t_curRegion.Equals(startRegion))
                {
                    finalRegionList.Add(t_curRegion);
                    t_curRegion = t_curRegion.ParentRegion;
                }

                finalRegionList.Add(startRegion);
                finalRegionList.Reverse();

                return;
            }

            for (int i = 0; i < curRegion.AdjRegion.Length; i++)
                OpenRegionListAdd(curRegion.AdjRegion[i]);
        }
    }

    private void OpenRegionListAdd(Region p_region)
    {
        if (closedRegionList.Contains(p_region)) return;

        var t_moveCost = CalculateWeight(curRegion, p_region);
        if (openRegionList.Contains(p_region) && curRegion.G + t_moveCost > p_region.G) return;

        p_region.ParentRegion = curRegion;
        p_region.G = t_moveCost;
        p_region.H = CalculateWeight(p_region, targetRegion);

        openRegionList.Add(p_region);
    }

    private float CalculateWeight(Region p_fromRegion, Region p_toRegion)
    {
        var dx = Mathf.Abs(p_fromRegion.TargetPos.x - p_toRegion.TargetPos.x);
        var dy = Mathf.Abs(p_fromRegion.TargetPos.y - p_toRegion.TargetPos.y);

        return Mathf.Abs(dx - dy) * 10f + Mathf.Min(dx, dy) * 14f;
    }
}
