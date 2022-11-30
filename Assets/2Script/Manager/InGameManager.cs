using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InGameManager : MonoBehaviour
{
    private UIManager manager = null;
    private InGameUIGroup inGameUI = null;
    private FadeUIGroup fadeUI = null;

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
        manager = UIManager.Instance;
        inGameUI = manager.InGameUI;
        fadeUI = manager.FadeUI;

        StartCoroutine(Init());
    }

    private IEnumerator Init()
    {
        manager.ActiveUI(EUIList.FADE);
        fadeUI.InitUI();

        yield return fadeUI.FadeIn();

        manager.ActiveUI(EUIList.INGAME);
        inGameUI.SetControl();
        gamePlayer.InitPlayer();
        inGameUI.InitUI();
    }

    public List<Region> FindRegion(Region p_startRegion)
    {
        return pathController.FindPath(p_startRegion, targetRegion);
    }
}
