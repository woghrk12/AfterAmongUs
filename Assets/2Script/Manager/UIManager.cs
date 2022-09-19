using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

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

	[SerializeField]private JoyStick joystick = null;
	[SerializeField] private Image screen = null;

	public JoyStick Joystick { get { return joystick; } }

	private void Awake()
	{
		var objs = FindObjectsOfType<UIManager>();

		if (objs.Length != 1)
		{
			Destroy(gameObject);
			return;
		}

		DontDestroyOnLoad(gameObject);

		joystick = GetComponentInChildren<JoyStick>();
	}

	public static void ActivateJoystick() => Instance.joystick.gameObject.SetActive(true);
	public static void DisableJoystick() => Instance.joystick.gameObject.SetActive(false);
	public static void FadeIn() => instance.StartCoroutine(instance.FadeInCo());
	public static void FadeOut() => instance.StartCoroutine(instance.FadeOutCo());

	private IEnumerator FadeInCo()
	{
		if (!screen.gameObject.activeSelf) screen.gameObject.SetActive(true);
		yield return ChangeScreen(true);
		screen.gameObject.SetActive(false);
	}
	private IEnumerator FadeOutCo()
	{
		if (!screen.gameObject.activeSelf) screen.gameObject.SetActive(true);
		yield return ChangeScreen(false);
	}

	private IEnumerator ChangeScreen(bool p_isFadeIn)
	{
		float t_timer = 0f;
		float t_totalTime = 1f;
		Color t_color = screen.color;

		while (t_timer <= 1f)
		{
			t_color.a = Mathf.Lerp(p_isFadeIn ? 1f : 0f, p_isFadeIn ? 0f : 1f, t_timer / t_totalTime);
			screen.color = t_color;
			t_timer += Time.deltaTime;
			yield return null;
		}
	}
}

