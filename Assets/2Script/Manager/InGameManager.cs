using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InGameManager : MonoBehaviour
{
	public static InGameManager instance;

	public PointManager pointManager;

	[SerializeField] private List<Region> enemySpawnRegions;

	private int enemyCount;
	[SerializeField] private ControlSlider progress;

	[SerializeField] private GamePlayer player;

	[SerializeField] private Image screen;
	[SerializeField] private GameObject pointLight;
	[SerializeField] private GameObject globalLight;

	private int stage;
	[SerializeField] private Text stageText;

	[SerializeField] private Text timerText;

	private void Awake()
	{
		instance = this;

		player = GameObject.FindGameObjectWithTag("Player").GetComponent<GamePlayer>();

		stage = 1;
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
			StartCoroutine(SpawnPortal(1));
		if (Input.GetKeyDown(KeyCode.F2))
			StartCoroutine(SpawnEnemy(1));
		if (Input.GetKeyDown(KeyCode.F3))
			ItemManager.SpawnItems(1);
    }

    private IEnumerator SpawnEnemy(int p_num)
	{
		yield return new WaitForSeconds(1f);

		for (int i = 0; i < p_num; i++)
		{
			var t_regions = enemySpawnRegions;
			t_regions.Remove(player.playerRegion);
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
			t_regions.Remove(player.playerRegion);
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

	private IEnumerator StageChange(bool p_isStart)
	{
		float t_time = 0f;
		Color t_color = Color.black;

		while (t_time <= 1f)
		{
			t_color.a = t_time;
			screen.color = t_color;
			t_time += 0.1f;
			yield return new WaitForSeconds(0.1f);
		}

		ChangeLight(p_isStart);

		while (t_time >= 0f)
		{
			t_color.a = t_time;
			screen.color = t_color;
			t_time -= 0.1f;
			yield return new WaitForSeconds(0.1f);
		}
	}

	private IEnumerator WaveStart(float p_time)
	{
		stageText.text = "WAVE  " + stage.ToString();
		ItemManager.SpawnItems(10);

		yield return TimeCheck(p_time);

		ItemManager.ReturnItems();
		enemyCount = stage * 2;
		progress.SetMaxValue(enemyCount);

		yield return StageChange(true);

		StartCoroutine(SpawnEnemy(enemyCount));
	}

	private IEnumerator WaveEnd()
	{
		yield return StageChange(false);

		stage++;

		StartCoroutine(WaveStart(5f));
	}

	private IEnumerator TimeCheck(float p_time)
	{
		timerText.gameObject.SetActive(true);

		var t_time = p_time;
		var t_interval = 1f;

		while (t_time > 10f)
		{
			t_time -= t_interval;
			timerText.text = t_time.ToString();

			yield return new WaitForSeconds(t_interval);
		}

		while (t_time > 0f)
		{
			t_time -= t_interval;

			timerText.text = t_time.ToString();
			var t_colorValue = Mathf.Lerp(0f, 1f, t_time / 10f);

			timerText.color = new Color(1f, t_colorValue, t_colorValue);

			yield return new WaitForSeconds(t_interval);
		}

		timerText.gameObject.SetActive(false);
	}
}
