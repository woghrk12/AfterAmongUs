using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
	private static GameManager instance = null;
	public static GameManager Instance
	{
		get
		{
			if (instance == null)
			{
				var t_obj = FindObjectOfType<GameManager>();

				if (t_obj != null)
				{
					instance = t_obj;
				}
				else
				{
					var t_newObj = new GameObject().AddComponent<GameManager>();
					instance = t_newObj;
				}
			}
			return instance;
		}
	}

	public static EPlayerColor playerColor = EPlayerColor.RED;

	private void Awake()
	{
		var t_objs = FindObjectsOfType<GameManager>();

		if (t_objs.Length != 1)
		{
			Destroy(gameObject);
			return;
		}

		DontDestroyOnLoad(gameObject);

	}

}
