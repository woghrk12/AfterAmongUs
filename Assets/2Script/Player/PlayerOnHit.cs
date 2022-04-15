using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerOnHit : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Region")
        {
            GameManager.Instance.playerRegion = collision.GetComponent<Region>();
        }
    }
}
