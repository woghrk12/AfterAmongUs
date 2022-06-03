using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
	public static GameManager instance;

	public PointManager pointManager;

	[SerializeField] private List<Region> enemySpawnRegions;

	private int enemyCount;
	[SerializeField] private ControlSlider progress;
	
	[SerializeField] private PlayerBehavior player;

	[SerializeField] private Image screen;
	[SerializeField] private GameObject pointLight;
	[SerializeField] private GameObject globalLight;

	private int stage;
	[SerializeField] private Text stageText;

	[SerializeField] private Text timerText;

	private void Awake()
	{
		instance = this;

		player = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerBehavior>();

		stage = 1;
	}

    private void Start()
    {
		StartCoroutine(WaveStart(5f));
    }

    private IEnumerator SpawnEnemy(int p_num)
	{
		yield return new WaitForSeconds(1f);

		for (int i = 0; i < enemyCount; i++)
		{
			var t_regions = enemySpawnRegions;
			t_regions.Remove(player.playerRegion);						
			var t_spawnRegion = t_regions[Random.Range(0, t_regions.Count)];
			var t_enemy = ObjectPooling.SpawnObject("Enemy", Vector3.zero, Quaternion.identity);
			t_enemy.GetComponent<EnemyBehaviour>().SetEnemy(t_spawnRegion);
		}
	}

	public void EnemyDie()
	{
		enemyCount--;
		progress.SetValue(enemyCount);

		if (enemyCount <= 0)
			StartCoroutine(WaveEnd());
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
