using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Cinemachine;

public class TitleManager : MonoBehaviour
{
    [SerializeField] private GameObject title;
    [SerializeField] private float startShakeIntensity;

    [SerializeField] private Button gameStartButton;

    [SerializeField] private CinemachineVirtualCamera virtualCamera;

    private void Start()
    {
        CameraShaking.SetCameraShake(startShakeIntensity);
    }

    private IEnumerator GameStart()
    {
        gameStartButton.GetComponent<Animator>().SetTrigger("FadeOut");
        gameStartButton.enabled = false;

        yield return new WaitForSeconds(0.5f);
        
        title.GetComponent<Animator>().SetTrigger("FadeOut");

        yield return new WaitForSeconds(0.5f);

        CameraShaking.SetCameraShake(startShakeIntensity / 2f);
        var timer = 0f;
        var totaltime = 1f;
        while (timer <= 1f)
        {
            virtualCamera.m_Lens.OrthographicSize = Mathf.Lerp(5f, 2.5f, timer / totaltime);
            timer += Time.deltaTime;
            yield return null;
        }
    }

    public void OnClickStartButton()
    {
        StartCoroutine(GameStart());
    }
}
