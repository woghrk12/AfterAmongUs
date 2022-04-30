using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerWeapon : MonoBehaviour
{
    private SpriteRenderer sprite;
    private Camera mainCamera;

    private Quaternion rotation;

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

        var direction = isLeft ? oPosition - mPosition : mPosition - oPosition;
                
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
            
        if (angle < -80f || angle > 80f)    
        {
            angle = Mathf.Clamp(angle, -80f, 80f);
        }

        rotation = Quaternion.AngleAxis(angle, Vector3.forward);
        transform.rotation = rotation;
    }

    public void Shot()
    {
        Instantiate(bullet, firePosition.position, rotation);
        bullet.SetActive(true);
    }
}

