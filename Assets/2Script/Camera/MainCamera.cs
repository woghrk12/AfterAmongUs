using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class MainCamera : MonoBehaviour
{
    [SerializeField] private int width, height;
    [SerializeField] private float limitMinX, limitMaxX, limitMinY, limitMaxY;

    private Transform player;

    [SerializeField] private CinemachineVirtualCamera virtualCamera;
    private CinemachineBasicMultiChannelPerlin virtualCameraNoise;
    private float startIntensity = 0f;
    private float shakeTimer = 0f;
    private float totalShakeTime = 0f;

    private void Awake()
    {
        Camera t_camera = GetComponent<Camera>();

        Rect t_rect = t_camera.rect;
        float t_scaleHeight = ((float)Screen.width / Screen.height) / ((float)width / height);
        float t_scaleWidth = 1f / t_scaleHeight;

        if (t_scaleHeight < 1)
        {
            t_rect.height = t_scaleHeight;
            t_rect.y = (1f - t_scaleHeight) / 2f;
        }
        else
        {
            t_rect.width = t_scaleWidth;
            t_rect.x = (1f - t_scaleWidth) / 2f;

        }

        t_camera.rect = t_rect;

        player = GameObject.FindGameObjectWithTag("Player").transform;

        virtualCameraNoise = virtualCamera.GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>();
    }

    private void Update()
    {
        CameraShake();
    }

    private void LateUpdate()
    {
        Follow(player);
    }

    private void Follow(Transform p_transform)
    {
        float t_x = Mathf.Clamp(p_transform.position.x, limitMinX, limitMaxX);
        float t_y = Mathf.Clamp(p_transform.position.y, limitMinY, limitMaxY);
        transform.position = new Vector3(t_x, t_y, -10);
    }

    public void SetCameraShake(float p_intensity, float p_time)
    {
        startIntensity = p_intensity;
        shakeTimer = p_time;
        totalShakeTime = p_time;
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
