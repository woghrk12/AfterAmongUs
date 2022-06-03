using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class CameraShaking : MonoBehaviour
{
    [SerializeField] private CinemachineVirtualCamera virtualCamera;
    private CinemachineBasicMultiChannelPerlin virtualCameraNoise;
    private float startIntensity = 0f;
    private float shakeTimer = 0f;
    private float totalShakeTime = 0f;

     private bool isInfinite;

    private void Awake()
    {
        virtualCameraNoise = virtualCamera.GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>();
    }

    private void Update()
    {
        if (isInfinite) return;

        CameraShake();
    }

    public void SetCameraShake(float p_intensity, float p_frequency)
    {
        virtualCameraNoise.m_AmplitudeGain = p_intensity;
        virtualCameraNoise.m_FrequencyGain = p_frequency;

        isInfinite = true;
    }

    public void SetCameraShake(float p_intensity, float p_frequency, float p_time)
    {
        startIntensity = p_intensity;
        shakeTimer = p_time;
        totalShakeTime = p_time;

        virtualCameraNoise.m_FrequencyGain = p_frequency;
        isInfinite = false;
    }

    private void CameraShake()
    {
        if (shakeTimer > 0f)
        {
            shakeTimer -= Time.deltaTime;

            virtualCameraNoise.m_AmplitudeGain = Mathf.Lerp(0f, startIntensity, shakeTimer / totalShakeTime);
        }
    }
}
