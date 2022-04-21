using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerOnHit : MonoBehaviour
{
    [SerializeField] private PlayerBehavior player;

    private void Start()
    {
        player = GetComponentInParent<PlayerBehavior>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Region")
        {
            player.playerRegion = collision.GetComponent<Region>();
        }
    }
}
