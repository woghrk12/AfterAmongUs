using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InGameManager : MonoBehaviour
{
    [SerializeField] private GamePlayer gamePlayer = null;
    public GamePlayer GamePlayer { get { return gamePlayer; } }

    private PathFindingByRegion pathController = null;

    [SerializeField] private Region targetRegion = null;

    private void Awake()
    {
        pathController = new PathFindingByRegion();
    }

    private void Start()
    {
        var t_uiManager = UIManager.Instance;
        var t_inGameUI = t_uiManager.InGameUI;

        t_uiManager.ActiveUI(EUIList.INGAME);
        t_inGameUI.SetControl();
        gamePlayer.InitPlayer();
        t_inGameUI.InitUI();
    }

    public List<Region> FindRegion(Region p_startRegion)
    {
        return pathController.FindPath(p_startRegion, targetRegion);
    }
}
