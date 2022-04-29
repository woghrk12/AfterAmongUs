using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerWeapon : MonoBehaviour
{
    private SpriteRenderer sprite;
    private Camera mainCamera;

    private Vector3 direction;

    [SerializeField] private Transform firePosition;
    [SerializeField] private GameObject bullet;

    private void Awake()
    {
        sprite = GetComponent<SpriteRenderer>();
        mainCamera = Camera.main;
    }

    private void Update()
    {
        Look();
    }

    private void Look()
    {
        var mPosition = mainCamera.ScreenToWorldPoint(Input.mousePosition);
        var oPosition = transform.position;

        var isLeft = mPosition.x < oPosition.x;
        sprite.flipX = isLeft;

        direction = isLeft ? oPosition - mPosition : mPosition - oPosition;
                
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
            
        if (angle < -80f || angle > 80f)    
        {
            angle = Mathf.Clamp(angle, -80f, 80f);
        }

        var rotation = Quaternion.AngleAxis(angle, Vector3.forward);
        transform.rotation = rotation;
    }

    public void Shot()
    {
        bullet.transform.position = firePosition.position;
        bullet.GetComponent<PlayerBullet>().direction = direction.normalized;
        bullet.SetActive(true);
    }
}

