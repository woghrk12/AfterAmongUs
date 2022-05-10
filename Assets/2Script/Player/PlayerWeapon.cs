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

    public float rate;
    public float recoil;

    private void Awake()
    {
        sprite = GetComponent<SpriteRenderer>();
        mainCamera = Camera.main;
    }

    private void Update()
    {
        Targeting();
    }

    private void Targeting()
    {
        var mPosition = mainCamera.ScreenToWorldPoint(Input.mousePosition);
        var oPosition = transform.position;

        var isLeft = mPosition.x < oPosition.x;
        
        transform.localScale = new Vector3(isLeft ? -1f : 1f, 1.5f, 1f);

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
        Instantiate(bullet, firePosition.position, firePosition.rotation);
        mainCamera.GetComponent<MainCamera>().SetCameraShake(recoil, 0.1f);
    }
}

