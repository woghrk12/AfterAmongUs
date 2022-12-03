using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Experimental.Rendering.Universal;

public class InGameManager : MonoBehaviour
{
    private UIManager manager = null;
    private InGameUIGroup inGameUI = null;
    private FadeUIGroup fadeUI = null;

    [SerializeField] private GamePlayer gamePlayer = null;
    public GamePlayer GamePlayer { get { return gamePlayer; } }

    private PathFindingByRegion pathController = null;

    private Mission mission = null;

    [SerializeField] private Light2D globalLight = null;
    [SerializeField] private Light2D pointLight = null;

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
        inGameUI.SetControl();
        gamePlayer.InitPlayer(inGameUI);
        inGameUI.InitUI();

        manager.ActiveUI(EUIList.FADE);
        fadeUI.InitUI();

        yield return fadeUI.FadeIn();

        manager.ActiveUI(EUIList.INGAME);
    }

    public List<Region> FindRegion(Region p_startRegion)
    {
        return pathController.FindPath(p_startRegion, mission.Region);
    }

    public IEnumerator StartMission(Mission p_mission)
    {
        mission = p_mission;

        fadeUI.gameObject.SetActive(true);
        yield return fadeUI.FadeOut();
        
        mission.TryEffect();
        ChangeLight(false);

        yield return fadeUI.FadeIn();
        fadeUI.gameObject.SetActive(false);
    }

    private void ChangeLight(bool p_isGlobal)
    {
        globalLight.gameObject.SetActive(p_isGlobal);
        pointLight.gameObject.SetActive(!p_isGlobal);
    }
}
