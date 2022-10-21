using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InGameManager : MonoBehaviour
{
    private PathFindingByRegion pathController = null;

    [SerializeField] private Region targetRegion = null;

    private void Awake()
    {
        pathController = new PathFindingByRegion();
    }

    public List<Region> FindRegion(Region p_startRegion)
    {
        return pathController.FindPath(p_startRegion, targetRegion);
    }
}
