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

        if (collision.tag == "Enemy")
        {
            StartCoroutine(OnDamageCo());
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.tag == "Region")
        {
            player.playerRegion = null;
        }
    }

    private IEnumerator OnDamageCo()
    {
        gameObject.layer = 8;
        int countTime = 0;

        while (countTime < 10)
        {
            if (countTime % 2 == 0)
                player.ChangeColor(new Color(1f, 1f, 1f, 0.3f));
            else
                player.ChangeColor(new Color(1f, 1f, 1f, 0.6f));

            yield return new WaitForSeconds(0.2f);

            countTime++;
        }

        player.ChangeColor(new Color(1f, 1f, 1f, 1f));
        
        gameObject.layer = 7;
    }
}
