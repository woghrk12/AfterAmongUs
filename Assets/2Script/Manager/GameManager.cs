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

	public static EPlayerColor playerColor;
	public static EControlType controlType = EControlType.END;

	private void Awake()
	{
		var objs = FindObjectsOfType<GameManager>();

		if (objs.Length != 1)
		{
			Destroy(gameObject);
			return;
		}

		DontDestroyOnLoad(gameObject);

#if UNITY_EDITOR
		controlType = EControlType.KEYBOARD;
#endif
#if UNITY_ANDROID
		controlType = EControlType.JOYSTICK;
#endif
	}
}

