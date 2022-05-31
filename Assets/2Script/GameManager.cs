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

	[SerializeField] private UnityEngine.UI.Image screen;
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
		StartCoroutine(StageStart(5f));
    }

    private IEnumerator SpawnEnemy(int num)
	{
		yield return new WaitForSeconds(1f);

		for (int i = 0; i < enemyCount; i++)
		{
			var tempList = enemySpawnRegions;
			tempList.Remove(player.playerRegion);						
			var spawnRegion = tempList[Random.Range(0, tempList.Count)];
			var enemy = ObjectPooling.SpawnObject("Enemy", Vector3.zero, Quaternion.identity);
			enemy.GetComponent<EnemyBehaviour>().SetEnemy(spawnRegion);
		}
	}

	public void EnemyDie()
	{
		enemyCount--;
		progress.SetValue(enemyCount);

		if (enemyCount <= 0)
			StartCoroutine(StageEnd());
	}

	private void ChangeLight(bool isStart)
	{
		globalLight.SetActive(!isStart);
		pointLight.SetActive(isStart);
	}

	private IEnumerator StageChange(bool isStart)
	{
		float timer = 0f;
		Color screenColor = Color.black;

		while (timer <= 1f)
		{
			screenColor.a = timer;
			screen.color = screenColor;
			timer += 0.1f;
			yield return new WaitForSeconds(0.1f);
		}
		
		ChangeLight(isStart);
		
		while (timer >= 0f)
		{
			screenColor.a = timer;
			screen.color = screenColor;
			timer -= 0.1f;
			yield return new WaitForSeconds(0.1f);
		}
	}

	private IEnumerator StageStart(float timer)
	{
		stageText.text = "STAGE  " + stage.ToString();
		ItemManager.SpawnItems(10);

		yield return TimeCheck(timer);

		ItemManager.ReturnItems();
		enemyCount = stage * 2;
		progress.SetMaxValue(enemyCount);

		yield return StageChange(true);
		
		StartCoroutine(SpawnEnemy(enemyCount));
	}

	private IEnumerator StageEnd()
	{
		yield return StageChange(false);
		
		stage++;

		StartCoroutine(StageStart(5f));
	}

	private IEnumerator TimeCheck(float time)
	{
		timerText.gameObject.SetActive(true);

		var leftTime = time;
		var interval = 1;
		
		while (leftTime > 0)
		{
			leftTime -= interval;
			
			timerText.text = leftTime.ToString();
			var colorValue = Mathf.Lerp(0f, 1f, leftTime / time);
			
			timerText.color = new Color(1f, colorValue, colorValue);

			yield return new WaitForSeconds(interval);
		}

		timerText.gameObject.SetActive(false);
	}
}
