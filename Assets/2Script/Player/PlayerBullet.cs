using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerBullet : MonoBehaviour
{
    public int damage;
    [SerializeField] private float speed;
    [SerializeField] private Vector2 direction;

    private void Awake()
    {
        direction = transform.up;
    }

    private void Update()
    {
        //transform.Translate(direction * speed * Time.deltaTime);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Wall")
            gameObject.SetActive(false);
    }
}
