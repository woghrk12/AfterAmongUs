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
    private Coroutine progress = null;

    [SerializeField] private int totalNum = 0;
    [SerializeField] private int targetNum = 0;
    private int successNum = 0;
    private int failNum = 0;

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
        inGameUI.StatusUI.SetMissionStatus(targetNum);

        manager.ActiveUI(EUIList.FADE);
        fadeUI.InitUI();

        yield return fadeUI.FadeIn();

        manager.ActiveUI(EUIList.INGAME);
    }

    public List<Region> FindRegion(Region p_startRegion)
    {
        return pathController.FindPath(p_startRegion, mission.Region);
    }

    public bool CheckCanStart(Mission p_mission)
    {
        if (!(progress is null)) {
            UIManager.Alert("The mission is already in progress!");
            return false;
        }
        
        progress = StartCoroutine(StartMission(p_mission));
        return true;
    }

    private IEnumerator StartMission(Mission p_mission)
    {
        mission = p_mission;
        mission.HitController.DieEvent += FailMission;
        mission.HitController.StartChecking();

        fadeUI.gameObject.SetActive(true);
        yield return fadeUI.FadeOut();
        
        mission.OnTry();
        ChangeLight(false);

        yield return fadeUI.FadeIn();
        fadeUI.gameObject.SetActive(false);

        yield return CountDown(mission.MissionTime);
        SuccessMission();
    }

    private void ChangeLight(bool p_isGlobal)
    {
        globalLight.gameObject.SetActive(p_isGlobal);
        pointLight.gameObject.SetActive(!p_isGlobal);
    }

    private IEnumerator CountDown(int p_time)
    {
        var t_time = p_time;
        inGameUI.InitTimer(t_time);

        while (t_time > 0)
        {
            inGameUI.ShowTimer(t_time);
            t_time -= 1;
            yield return Utilities.WaitForSeconds(1f);
        }

        inGameUI.ShowTimer(t_time);
    }

    public void SuccessMission()
    {
        successNum++;
        inGameUI.StatusUI.MissionStatus.SetValue(successNum);
        StartCoroutine(SuccessLight());
        mission.OnActive();
        EndMission();
    }

    public void FailMission()
    {
        failNum++;
        StartCoroutine(FailLight());
        EndMission();
    }

    private void EndMission()
    {
        if (!(progress is null)) StopCoroutine(progress);

        mission.HitController.StopChecking();
        mission.HitController.DieEvent -= FailMission;
        mission = null;
        progress = null;
        inGameUI.EndTimer();
        ChangeLight(true);
    }

    private IEnumerator SuccessLight()
    {
        yield return BlinkColorLight(Color.green);
    }

    private IEnumerator FailLight()
    {
        for(int i = 0; i < 3; i++)
            yield return BlinkColorLight(Color.red);
    }

    private IEnumerator BlinkColorLight(Color p_color)
    {
        var t_timer = 0f;
        var t_totalTime = 1f;

        var t_color = globalLight.color;

        while (t_timer <= t_totalTime)
        {
            globalLight.color = Color.Lerp(t_color, p_color, t_timer / t_totalTime);
            t_timer += Time.deltaTime;
            yield return null;
        }

        while (t_timer >= 0f)
        {
            globalLight.color = Color.Lerp(t_color, p_color, t_timer / t_totalTime);
            t_timer -= Time.deltaTime;
            yield return null;
        }

        globalLight.color = t_color;
    }
}
