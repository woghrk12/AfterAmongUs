using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FadeUI : MonoBehaviour
{
	[SerializeField] private Image screen = null;

	public IEnumerator FadeIn() => ChangeScreen(true);

	public IEnumerator FadeOut() => ChangeScreen(false);

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
