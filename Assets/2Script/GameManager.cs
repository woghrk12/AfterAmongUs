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
	[SerializeField] private PlayerBehavior player;

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
	}

    private void Start()
    {
		StartCoroutine(SpawnEnemy());
    }

	IEnumerator SpawnEnemy()
	{
		yield return new WaitForSeconds(3f);

		for (int i = 0; i < enemy.Count; i++)
		{
			var tempList = enemySpawnRegions;
			tempList.Remove(player.playerRegion);						
			var spawnRegion = tempList[Random.Range(0, tempList.Count)];
			enemy[i].GetComponent<EnemyBehaviour>().SetEnemy(spawnRegion);
		}
	}
}
