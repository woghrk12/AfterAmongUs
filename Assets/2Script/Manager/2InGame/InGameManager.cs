using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InGameManager : MonoBehaviour
{
    private PathFindingByRegion pathController = null;

    private void Awake()
    {
        pathController = new PathFindingByRegion();
    }
}
