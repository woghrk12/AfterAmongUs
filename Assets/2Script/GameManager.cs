using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
	private static GameManager instance;
	public static GameManager Instance
	{
		get
		{
			if (instance == null)
			{
				var obj = FindObjectOfType<GameManager>();

				if (obj != null)
				{
					instance = obj;
				}
				else
				{
					var newObj = new GameObject().AddComponent<GameManager>();
					instance = newObj;
				}
			}

			return instance;
		}
	}

	public PointManager pointManager;

	[SerializeField] private List<Region> enemySpawnRegions;
	[SerializeField] private List<GameObject> enemy;
	private int enemyCount;
	[SerializeField] private ControlSlider progress;
	
	[SerializeField] private PlayerBehavior player;

	[SerializeField] private GameObject pointLight;
	[SerializeField] private GameObject globalLight;

	private void Awake()
	{
		var objs = FindObjectsOfType<GameManager>();

		if (objs.Length != 1)
		{
			Destroy(gameObject);
			return;
		}

		DontDestroyOnLoad(gameObject);

		player = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerBehavior>();

		enemyCount = enemy.Count;
		progress.SetMaxValue(enemyCount);
	}

    private void Start()
    {
		StartCoroutine(SpawnEnemy());
    }

    private void Update()
    {
		if (Input.GetKeyDown(KeyCode.Q))
		{
			ChangeLight(true);	
		}
		if (Input.GetKeyUp(KeyCode.Q))
		{
			ChangeLight(false);
		}
    }

    private IEnumerator SpawnEnemy()
	{
		yield return new WaitForSeconds(1f);

		for (int i = 0; i < enemyCount; i++)
		{
			var tempList = enemySpawnRegions;
			tempList.Remove(player.playerRegion);						
			var spawnRegion = tempList[Random.Range(0, tempList.Count)];
			enemy[i].GetComponent<EnemyBehaviour>().SetEnemy(spawnRegion);
		}
	}

	public void EnemyDie()
	{
		enemyCount--;
		progress.SetValue(enemyCount);
	}

	private void ChangeLight(bool stageStart)
	{
		globalLight.SetActive(!stageStart);
		pointLight.SetActive(stageStart);
	}
}
