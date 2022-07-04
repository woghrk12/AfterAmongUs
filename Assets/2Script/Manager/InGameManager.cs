using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Experimental.Rendering.Universal;

public class InGameManager : MonoBehaviour
{
	public static InGameManager instance;
	public static IMission missionInProgress;

	[SerializeField] private List<Region> enemySpawnRegions;

	[SerializeField] private ControlSlider progress;

	[SerializeField] private GamePlayer player;
	private static Region playerRegion;

	[SerializeField] private GameObject pointLight;
	[SerializeField] private GameObject globalLight;

	private void Awake()
	{
		instance = this;

		player = GameObject.FindGameObjectWithTag("Player").GetComponent<GamePlayer>();

		missionInProgress = null;
	}

	private void Start()
	{
		player.SetColor(GameManager.playerColor);
		//StartCoroutine(WaveStart(5f));
		player.CanMove = true;
	}

    private void Update()
    {
		if (Input.GetKeyDown(KeyCode.F1))
			StartCoroutine(SpawnPortal(4));
		if (Input.GetKeyDown(KeyCode.F2))
			StartCoroutine(SpawnEnemy(4));
		if (Input.GetKeyDown(KeyCode.F3))
			ItemManager.SpawnItems(1);	
    }

	public static void SetPlayerRegion(Region p_region)
	{
		playerRegion = p_region;
	}

	public static Region GetPlayerRegion()
	{
		return playerRegion;
	}

	private IEnumerator SpawnEnemy(int p_num)
	{
		yield return new WaitForSeconds(1f);

		for (int i = 0; i < p_num; i++)
		{
			var t_regions = enemySpawnRegions;
			t_regions.Remove(playerRegion);
			var t_spawnRegion = t_regions[Random.Range(0, t_regions.Count)];
			var t_enemy = ObjectPooling.SpawnObject("EnemyNormal", Vector3.zero, Quaternion.identity).GetComponent<EnemyNormal>();
			t_enemy.SetEnemy(t_spawnRegion);
		}
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
}
