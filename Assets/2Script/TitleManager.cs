using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TitleManager : MonoBehaviour
{
    [SerializeField] private GameObject title;
    [SerializeField] private float startShakeIntensity;
    private void Start()
    {
        CameraShaking.SetCameraShake(startShakeIntensity);
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Q))
        {
            GameStart();
        }
    }

    private void GameStart()
    {
        title.GetComponent<Animator>().SetTrigger("FadeOut");
    }
}
