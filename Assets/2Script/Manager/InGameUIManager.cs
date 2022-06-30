using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InGameUIManager : MonoBehaviour
{
    public static InGameUIManager instance;

    [SerializeField] private Image screen;
    [SerializeField] private Text timerText;

    private void Awake()
    {
        instance = this;
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

    public static IEnumerator FadeIn() => instance.ChangeScreen(true);
    public static IEnumerator FadeOut() => instance.ChangeScreen(false);

    private IEnumerator TimeCheckCo(float p_time)
    {
        timerText.gameObject.SetActive(true);

        var t_time = p_time;
        var t_interval = 1f;

        while (t_time > 10f)
        {
            t_time -= t_interval;
            timerText.text = t_time.ToString();

            yield return new WaitForSeconds(t_interval);
        }

        while (t_time > 0f)
        {
            t_time -= t_interval;

            timerText.text = t_time.ToString();
            var t_colorValue = Mathf.Lerp(0f, 1f, t_time / 10f);

            timerText.color = new Color(1f, t_colorValue, t_colorValue);

            yield return new WaitForSeconds(t_interval);
        }

        timerText.gameObject.SetActive(false);
    }

    public static IEnumerator TimeCheck(float p_time) => instance.TimeCheckCo(p_time);
}
