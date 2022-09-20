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
				var t_obj = FindObjectOfType<UIManager>();

				if (t_obj != null)
				{
					instance = t_obj;
				}
				else
				{
					var t_newObj = new GameObject().AddComponent<UIManager>();
					instance = t_newObj;
				}
			}
			return instance;
		}
	}

	[SerializeField] private GameObject playerUI = null;
	[SerializeField] private JoyStick joystick = null;
	[SerializeField] private Button useButton = null;
	[SerializeField] private Image screen = null;

	public JoyStick Joystick { get { return joystick; } }
	public Button UseButton { get { return useButton; } }

	private void Awake()
	{
		var t_objs = FindObjectsOfType<UIManager>();

		if (t_objs.Length != 1)
		{
			Destroy(gameObject);
			return;
		}

		DontDestroyOnLoad(gameObject);

		joystick = GetComponentInChildren<JoyStick>();
	}

	public static void ActivatePlayerUI() => Instance.playerUI.SetActive(true);
	public static void DisablePlayerUI() => Instance.playerUI.SetActive(false);
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

