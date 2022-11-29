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

	[SerializeField]private TitleUI titleUI = null;
	public TitleUI TitleUI { get { return titleUI; } }

	[SerializeField] private GameObject playerUI = null;
	[SerializeField] private JoyStick joystick = null;
	[SerializeField] private Button useButton = null;
	[SerializeField] private Image screen = null;
	[SerializeField] private Text alertText = null;

	public JoyStick Joystick { get { return TitleUI.JoyStick; } }
	public Button UseButton { get { return TitleUI.UseButton; } }

	private Coroutine alertTextCo = null;

	private void Awake()
	{
		var t_objs = FindObjectsOfType<UIManager>();

		if (t_objs.Length != 1)
		{
			Destroy(gameObject);
			return;
		}

		DontDestroyOnLoad(gameObject);
	}

	public static void ActivatePlayerUI() => Instance.playerUI.SetActive(true);
	public static void DisablePlayerUI() => Instance.playerUI.SetActive(false);
	public static void FadeIn() => Instance.StartCoroutine(Instance.FadeInCo());
	public static void FadeOut() => Instance.StartCoroutine(Instance.FadeOutCo());
	public static void Alert(string p_text) => Instance.ShowAlertText(p_text);

	public void OnScreen()
	{
		if (screen.gameObject.activeSelf) return;
		screen.gameObject.SetActive(true);
	}
	public void OffScreen()
	{
		if (!screen.gameObject.activeSelf) return;
		screen.gameObject.SetActive(false);
	}

	private IEnumerator FadeInCo()
	{
		OnScreen();
		yield return ChangeScreen(true);
		OffScreen();
	}
	private IEnumerator FadeOutCo()
	{
		OnScreen();
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

	private void ShowAlertText(string p_text)
	{
		if (alertTextCo != null) StopCoroutine(alertTextCo);
		alertTextCo = StartCoroutine(ShowAlertTextCo(p_text));
	}

	private IEnumerator ShowAlertTextCo(string p_text)
	{
		alertText.gameObject.SetActive(true);
		alertText.text = p_text;

		var t_color = alertText.color;
		var t_timer = 0f;
		var t_totalTime = 1f;

		t_color.a = 1f;
		alertText.color = t_color;

		yield return new WaitForSeconds(2f);

		while (t_timer < t_totalTime)
		{
			t_color.a = Mathf.Lerp(1f, 0f, t_timer / t_totalTime);
			alertText.color = t_color;
			t_timer += 0.05f;
			yield return new WaitForSeconds(0.05f);
		}

		alertText.gameObject.SetActive(false);
		alertTextCo = null;
	}
}

