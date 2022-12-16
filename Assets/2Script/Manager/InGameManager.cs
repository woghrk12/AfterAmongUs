using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Experimental.Rendering.Universal;

public class InGameManager : MonoBehaviour
{
    public static InGameManager instance = null;
    public static readonly int NumTotalEnemy = 50;
    public static int enemyNum = 0;
    private UIManager manager = null;
    private InGameUIGroup inGameUI = null;
    private FadeUIGroup fadeUI = null;

    [SerializeField] private GamePlayer gamePlayer = null;
    public GamePlayer GamePlayer { get { return gamePlayer; } }

    private PathFindingByRegion pathController = null;
    [SerializeField] private Region[] regionList = null;

    private Mission mission = null;
    private Coroutine progress = null;

    [SerializeField] private int totalNum = 0;
    [SerializeField] private int targetNum = 0;
    private int successNum = 0;
    private int failNum = 0;

    [SerializeField] private Light2D globalLight = null;
    [SerializeField] private Light2D pointLight = null;

    [SerializeField] private GameObject prefabPortal = null;

    private void Awake()
    {
        instance = this;
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

        yield return Utilities.WaitForSeconds(1f);

        ObjectPoolingManager.instance.InitPool();

        yield return fadeUI.FadeIn();

        manager.ActiveUI(EUIList.INGAME);
    }

    public List<Region> FindRegion(Region p_startRegion)
    {
        return pathController.FindPath(p_startRegion, regionList[(int)mission.Region]);
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

        SpawnEnemyPortal(mission.Region, 5);
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

    private void SpawnEnemyPortal(ERegion p_targetRegion, int p_portalNum)
    {
        var t_totalNum = (int)ERegion.END;
        var t_portalNum = p_portalNum > t_totalNum ? t_totalNum : p_portalNum;
        var t_targetRegion = (int)p_targetRegion;
        var t_spawnList = new int[t_totalNum];
        
        for (int i = 0; i < t_totalNum; i++)
            t_spawnList[i] = i;

        t_spawnList[t_targetRegion] = t_spawnList[t_totalNum - 1];
        t_totalNum--;

        for (int i = 0; i < t_portalNum; i++)
        {
            var t_random = Random.Range(0, t_totalNum);
            var t_spawnPos = regionList[t_spawnList[t_random]].TargetPos;
            ObjectPoolingManager.SpawnObject("EnemyPortal", new Vector3((float)t_spawnPos.x * 0.1f, (float)t_spawnPos.y * 0.1f, 0));
            t_spawnList[t_random] = t_spawnList[t_totalNum - 1];
            t_totalNum--;
        }
    }
}
