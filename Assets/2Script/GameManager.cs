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

	public Region playerRegion;
	public PointManager pointManager;

	[SerializeField] private List<Region> enemySpawnRegions;
	[SerializeField] private List<GameObject> enemy;

	private void Awake()
	{
		var objs = FindObjectsOfType<GameManager>();

		if (objs.Length != 1)
		{
			Destroy(gameObject);
			return;
		}

		DontDestroyOnLoad(gameObject);
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
			Region spawnRegion = enemySpawnRegions[Random.Range(0, enemySpawnRegions.Count)];
			enemy[i].SetActive(true);
			enemy[i].GetComponent<EnemyMove>().region = spawnRegion;
			enemy[i].transform.position = pointManager.GetPoint(spawnRegion.dstPoint).transform.position;
		}
		
	}
}
