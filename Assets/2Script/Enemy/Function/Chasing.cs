using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Chasing : MonoBehaviour
{
    private Vector3 target = Vector3.zero;
    public Vector3 Target { get { return target; } }

    public IEnumerator ChaseTarget(Transform p_target)
    {
        while (true)
        {
            target = p_target.position;
            yield return Utilities.WaitForSeconds(0.1f);
        }
    }

    public IEnumerator ChaseRegion(List<Region> p_regions) { yield return FindPath(p_regions); }

    private IEnumerator FindPath(List<Region> p_regions)
    {
        var t_regions = p_regions;
        var t_curRegion = t_regions[0];
        var t_curPos = new Vector2Int((int)(transform.position.x * 10f), (int)(transform.position.y * 10f));

        while (t_regions.Count > 0)
        {
            var t_targetPos = t_regions.Count > 1 
                ? t_curRegion.GetAdjPoint(t_regions[1]) 
                : t_curRegion.TargetPos;

            var t_nodes = t_curRegion.FindPath(t_curPos, t_targetPos);

            t_nodes.RemoveAt(0);

            while (t_nodes.Count > 0)
            {
                target = t_nodes[0].Position;
                yield return new WaitUntil(() => Utilities.CalculateDist(transform.position, t_nodes[0].Position) <= 0.1f);
                t_nodes.RemoveAt(0);
            }

            t_curPos = t_targetPos;

            t_regions.RemoveAt(0);

            if (t_regions.Count > 0) t_curRegion = t_regions[0];
        }
    }
}
