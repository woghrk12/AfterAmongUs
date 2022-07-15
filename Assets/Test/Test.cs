using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Test : MonoBehaviour
{

	private static Test instance;
	public static Test Instance
	{
		get
		{
			if (instance == null)
			{
				var obj = FindObjectOfType<Test>();

				if (obj != null)
				{
					instance = obj;
				}
				else
				{
					var newObj = new GameObject().AddComponent<Test>();
					instance = newObj;
				}
			}
			return instance;
		}
	}

    private void Awake()
    {
		var objs = FindObjectsOfType<Test>();

		if (objs.Length != 1)
		{
			Destroy(gameObject);
			return;
		}

		DontDestroyOnLoad(gameObject);
	}

    private void Update()
    {
        
    }

    public void OnClick()
    {
        LoadingManager.LoadScene(EScene.TITLE);
    }
}
