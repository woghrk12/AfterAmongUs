using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Experimental.Rendering.Universal;

public class InGameManager : MonoBehaviour
{
	public static InGameManager instance;
	private InGameUIManager inGameUIManager = null;

	public static Mission missionInProgress = null;
	private Coroutine missionCo = null;

	public static List<EnemyBehaviour> enemys;
	[SerializeField] private List<Region> enemySpawnRegions;

	[SerializeField] private ControlSlider progress;

	[SerializeField] private GamePlayer player;
	private static Region playerRegion;

	[SerializeField] private Light2D pointLight;
	[SerializeField] private Light2D globalLight;

	[SerializeField] private int numTotalMission = 0;
	[SerializeField] private int numNeedMission = 0;

	private int numFailMission = 0;
	public int NumFailMission
	{
		set
		{
			numFailMission = value;

			if (numFailMission >= numTotalMission - numNeedMission) instance.StartCoroutine(instance.EndGame());
		}
		get
		{
			return numFailMission;
		}
	}
	private int numCompleteMission = 0;
	public int NumCompleteMission
	{
		set
		{
			numCompleteMission = value;

			instance.progress.SetValue(numCompleteMission);

			if (numCompleteMission >= numNeedMission) instance.StartCoroutine(instance.EndGame());
			
		}
		get { return numCompleteMission; }
	}

	private void Awake()
	{
		instance = this;

		player = GameObject.FindGameObjectWithTag("Player").GetComponent<GamePlayer>();

		enemys = new List<EnemyBehaviour>();
	}

    private void Start()
    {
		inGameUIManager = InGameUIManager.instance;
	}

    private void OnEnable()
    {
		missionInProgress = null;
		NumCompleteMission = 0;
		NumFailMission = 0;

		progress.SetMaxValue(numNeedMission);
		progress.SetValue(NumCompleteMission);

		player.CanMove = true;
	}

    void Update()
	{
		if (Input.GetKeyDown(KeyCode.F2))
			StartCoroutine(SpawnPortal(2));
	}

	public static void SetPlayerRegion(Region p_region)
	{
		playerRegion = p_region;
	}

	public static Region GetPlayerRegion()
	{
		return playerRegion;
	}

	private IEnumerator SpawnPortal(int p_num)
	{
		yield return new WaitForSeconds(1f);

		for (int i = 0; i < p_num; i++)
		{
			var t_regions = enemySpawnRegions;
			t_regions.Remove(playerRegion);
			var t_spawnRegion = t_regions[Random.Range(0, t_regions.Count)];
			t_regions.Remove(t_spawnRegion);
			var t_portal = ObjectPooling.SpawnObject("EnemyPortal", Vector3.zero, Quaternion.identity).GetComponent<EnemyPortal>();
			t_portal.SetEnemy(t_spawnRegion);
			enemys.Add(t_portal);

			t_portal.miniMapObject = MiniMapManager.SpawnObject(t_portal.transform.position, EMiniMapObject.ENEMYPORTAL);
		}
	}

	private IEnumerator TurnOnPointLight()
	{
		var t_timer = 0f;
		var t_totalTime = 2f;

		while (t_timer <= t_totalTime)
		{
			globalLight.intensity = Mathf.Lerp(1f, 0f, t_timer / t_totalTime);
			pointLight.intensity = 1f - globalLight.intensity;
			t_timer += Time.deltaTime;
			yield return null;
		}
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
	}

	private IEnumerator TurnOnColorLight(Color p_color, int p_num = 1)
	{
		var t_num = p_num;

		while (t_num > 0)
		{
			yield return instance.BlinkColorLight(p_color);
			t_num--;
		}
	}

	public bool StartMission(Mission p_mission)
	{
		if (missionInProgress != null) return false;

		missionInProgress = p_mission;

		StartCoroutine(TurnOnPointLight());
		missionInProgress.SetMission(out playerRegion);

		inGameUIManager.SwitchTimeText(true);
		missionCo = StartCoroutine(missionInProgress.PerformMission());

		return true;
	}

	public void EndMission(bool p_isSuccess)
	{
		pointLight.intensity = 0f;
		globalLight.intensity = 1f;

		while (enemys.Count > 0)
		{
			enemys[0].IsDie = true;
			enemys.RemoveAt(0);
		}
		
		if (p_isSuccess) SuccessMission();
		else FailMission();

		inGameUIManager.SwitchTimeText(false);
		missionInProgress = null;
		missionCo = null;
	}

	private void SuccessMission()
	{
		NumCompleteMission++;
		StartCoroutine(TurnOnColorLight(new Color(0.6f, 1f, 0.6f)));
	}

	private void FailMission()
	{
		StopCoroutine(missionCo);

		NumFailMission++;
		StartCoroutine(TurnOnColorLight(new Color(1f, 0.5f, 0.5f), 3));
	}

	private IEnumerator EndGame()
	{
		yield return InGameUIManager.FadeOut();

		LoadingManager.LoadScene(EScene.ENDING);
	}
}

