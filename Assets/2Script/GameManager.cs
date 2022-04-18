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

	[SerializeField] private GameObject enemy;

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

		enemy.SetActive(true);
	}
}
