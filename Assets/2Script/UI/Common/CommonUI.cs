using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CommonUI : MonoBehaviour
{
    [SerializeField] private Text alertText = null;
	private Coroutine alertTextCo = null;

    public void Alert(string p_text)
    {
		if (alertTextCo != null) StopCoroutine(alertTextCo);
		alertTextCo = StartCoroutine(ShowAlertText(p_text));
    }

	private IEnumerator ShowAlertText(string p_text)
	{
		alertText.gameObject.SetActive(true);
		alertText.text = p_text;

		var t_color = alertText.color;
		var t_timer = 0f;
		var t_totalTime = 1f;

		t_color.a = 1f;
		alertText.color = t_color;

		yield return Utilities.WaitForSeconds(2f);

		while (t_timer < t_totalTime)
		{
			t_color.a = Mathf.Lerp(1f, 0f, t_timer / t_totalTime);
			alertText.color = t_color;
			t_timer += 0.05f;
			yield return Utilities.WaitForSeconds(0.05f);
		}

		alertText.gameObject.SetActive(false);
		alertTextCo = null;
	}
}
