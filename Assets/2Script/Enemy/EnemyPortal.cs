using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyPortal : MonoBehaviour
{
    [SerializeField] private Animator anim = null;
    [SerializeField] private SpriteRenderer sprite = null;

    [SerializeField] private Damagable hitController = null;

    [SerializeField] private GameObject enemyPrefab = null;

    private Coroutine spawnCo = null;

    private void OnEnable()
    {
        hitController.HitEvent += OnHit;
        hitController.DieEvent += OnDie;

        spawnCo = StartCoroutine(InitPortal());
    }

    private void OnDisable()
    {
        hitController.HitEvent -= OnHit;
        hitController.DieEvent -= OnDie;
    }

    private IEnumerator InitPortal()
    {
        yield return Utilities.WaitForSeconds(1f);

        hitController.StartChecking();
        
        while (true)
        {
            yield return Utilities.WaitForSeconds(Random.Range(5, 8));
            if (InGameManager.enemyNum > InGameManager.NumTotalEnemy) continue;
            SpawnEnemy();
            InGameManager.enemyNum++;
        }
    }

    private void SpawnEnemy()
    {
        var t_enemy = Instantiate(enemyPrefab, this.transform).GetComponent<Enemy>();
        StartCoroutine(t_enemy.InitEnemy(transform.position));
    }

    private void OnHit()
    {
        StartCoroutine(HitEffect());
    }

    private IEnumerator HitEffect()
    {
        sprite.color = Color.red;
        yield return Utilities.WaitForSeconds(0.05f);
        sprite.color = Color.white;
    }

    private void OnDie()
    {
        StopCoroutine(spawnCo);
        anim.SetTrigger("Die");
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!collision.CompareTag("Camera")) return;

        sprite.enabled = true;
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (!collision.CompareTag("Camera")) return;

        sprite.enabled = false;
    }
}
