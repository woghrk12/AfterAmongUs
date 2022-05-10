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
        Camera camera = GetComponent<Camera>();

        Rect rect = camera.rect;
        float scaleHeight = ((float)Screen.width / Screen.height) / ((float)width / height);
        float scaleWidth = 1f / scaleHeight;

        if (scaleHeight < 1)
        {
            rect.height = scaleHeight;
            rect.y = (1f - scaleHeight) / 2f;
        }
        else
        {
            rect.width = scaleWidth;
            rect.x = (1f - scaleWidth) / 2f;

        }

        camera.rect = rect;

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

    private void Follow(Transform target)
    {
        float x = Mathf.Clamp(target.position.x, limitMinX, limitMaxX);
        float y = Mathf.Clamp(target.position.y, limitMinY, limitMaxY);
        transform.position = new Vector3(x, y, -10);
    }

    public void SetCameraShake(float _intensity, float _time)
    {
        startIntensity = _intensity;
        shakeTimer = _time;
        totalShakeTime = _time;
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
