using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    public int damage;

    [SerializeField] private float speed;
    private Vector3 dir;

    private void Update()
    {
        transform.position += dir * speed * Time.deltaTime;
    }

    public void SetDirection(Vector3 p_dir)
    {
        dir = p_dir;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Wall"))
            ObjectPooling.ReturnObject(gameObject);
    }
}
