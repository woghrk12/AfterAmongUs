using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Experimental.Rendering.Universal;

public class InGameManager : MonoBehaviour
{
	public static InGameManager instance;
	public static Mission missionInProgress = null;
	public static List<EnemyBehaviour> enemys;

	[SerializeField] private List<Region> enemySpawnRegions;

	[SerializeField] private ControlSlider progress;

	[SerializeField] private GamePlayer player;
	private static Region playerRegion;

	[SerializeField] private GameObject pointLight;
	[SerializeField] private GameObject globalLight;

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
		player.SetColor(GameManager.playerColor);
    }

    private void OnEnable()
    {
		missionInProgress = null;
		NumCompleteMission = 0;
		NumFailMission = 0;

		progress.SetMaxValue(numNeedMission);
		progress.SetValue(NumCompleteMission);
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

	private void ChangeLight(bool p_isStart)
	{
		globalLight.SetActive(!p_isStart);
		pointLight.SetActive(p_isStart);
	}

	public static void TurnOnGlobalLight() => instance.ChangeLight(false);
	public static void TurnOnPointLight() => instance.ChangeLight(true);

	private IEnumerator BlinkColorLight(Color p_color)
	{
		var t_timer = 0f;
		var t_totalTime = 1f;
		
		var t_color = globalLight.GetComponent<Light2D>().color;
		
		while (t_timer <= t_totalTime) 
		{
			globalLight.GetComponent<Light2D>().color = Color.Lerp(t_color, p_color, t_timer / t_totalTime);
			t_timer += Time.deltaTime;
			yield return null;
		}

		while (t_timer >= 0f)
		{
			globalLight.GetComponent<Light2D>().color = Color.Lerp(t_color, p_color, t_timer / t_totalTime);
			t_timer -= Time.deltaTime;
			yield return null;
		}
	}

	public static IEnumerator TurnOnColorLight(Color p_color, int p_num = 1)
	{
		var t_num = p_num;

		while (t_num > 0)
		{
			yield return instance.BlinkColorLight(p_color);
			t_num--;
		}
	}

	private IEnumerator EndGame()
	{
		yield return InGameUIManager.FadeOut();

		LoadingManager.LoadScene(EScene.ENDING);
	}

}

