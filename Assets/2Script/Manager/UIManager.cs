using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIManager : MonoBehaviour
{
	private static UIManager instance = null;
	public static UIManager Instance
	{
		get
		{
			if (instance == null)
			{
				var obj = FindObjectOfType<UIManager>();

				if (obj != null)
				{
					instance = obj;
				}
				else
				{
					var newObj = new GameObject().AddComponent<UIManager>();
					instance = newObj;
				}
			}
			return instance;
		}
	}

	private JoyStick joyStick = null;
	public JoyStick JoyStick { get { return joyStick; } }

	private void Awake()
	{
		var objs = FindObjectsOfType<UIManager>();

		if (objs.Length != 1)
		{
			Destroy(gameObject);
			return;
		}

		DontDestroyOnLoad(gameObject);

		joyStick = GetComponentInChildren<JoyStick>();
	}
}
