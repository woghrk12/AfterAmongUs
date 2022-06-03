using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class CameraShaking : MonoBehaviour
{
    public static CameraShaking instance;

    [SerializeField] private CinemachineVirtualCamera virtualCamera;
    private CinemachineBasicMultiChannelPerlin virtualCameraNoise;
    private float startIntensity = 0f;
    private float shakeTimer = 0f;
    private float totalShakeTime = 0f;

     private bool isInfinite;

    private void Awake()
    {
        instance = Camera.main.GetComponent<CameraShaking>();

        virtualCameraNoise = virtualCamera.GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>();
    }

    private void Update()
    {
        if (isInfinite) return;

        CameraShake();
    }

    public static void SetCameraShake(float p_intensity)
        => instance.SetShakeValue(p_intensity, 0f, true);
    public static void SetCameraShake(float p_intensity, float p_time)
        => instance.SetShakeValue(p_intensity, p_time, false);

    private void SetShakeValue(float p_intensity, float p_time, bool p_isInfinite)
    {
        if (!p_isInfinite)
        {
            startIntensity = p_intensity;
            shakeTimer = p_time;
            totalShakeTime = p_time;
        }
        else
        {
            virtualCameraNoise.m_AmplitudeGain = p_intensity;
        }

        isInfinite = p_isInfinite;
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
