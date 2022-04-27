using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyOnHit : MonoBehaviour
{
    [SerializeField] private EnemyMove enemy;

    private void Awake()
    {
        enemy = GetComponentInParent<EnemyMove>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Region")
        {
            enemy.region = collision.GetComponent<Region>();
        }

        if (collision.tag == "Camera Collider")
        {
            enemy.sprite.enabled = true;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.tag == "Camera Collider")
        {
            enemy.sprite.enabled = false;
        }
    }
}
