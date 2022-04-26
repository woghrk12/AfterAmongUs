using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerOnHit : MonoBehaviour
{
    [SerializeField] private PlayerBehavior player;

    private void Awake()
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

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.tag == "Region")
        {
            player.playerRegion = null;
        }
    }
}
