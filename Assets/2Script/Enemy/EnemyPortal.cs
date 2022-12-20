using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyPortal : MonoBehaviour
{
    [SerializeField] private Animator anim = null;
    [SerializeField] private SpriteRenderer sprite = null;

    private InGameManager inGameManager = null;

    [SerializeField] private Damagable hitController = null;

    private Coroutine spawnCo = null;

    private void OnEnable()
    {
        hitController.HitEvent += OnHit;
        hitController.DieEvent += OnDie;
    }

    private void OnDisable()
    {
        hitController.HitEvent -= OnHit;
        hitController.DieEvent -= OnDie;
    }

    public void InitPortal(Vector3 p_position)
    {
        inGameManager = InGameManager.instance;
        inGameManager.enemyList.Add(hitController);
        inGameManager.enemyNum++;
        transform.position = p_position;
        anim.SetTrigger("Spawn");
        hitController.StartChecking();
        spawnCo = StartCoroutine(SpawnEnemy());        
    }

    private IEnumerator SpawnEnemy()
    {
        while (true)
        {
            yield return Utilities.WaitForSeconds(Random.Range(5, 8));
            if (inGameManager.enemyNum > inGameManager.NumTotalEnemy) continue;
            var t_enemy = ObjectPoolingManager.SpawnObject("EnemyNormal").GetComponent<Enemy>();
            StartCoroutine(t_enemy.InitEnemy(transform.position));
        }
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
        inGameManager.enemyNum--;
        inGameManager.enemyList.Remove(hitController);
        StartCoroutine(DieEffect());
    }

    private IEnumerator DieEffect()
    {
        anim.SetTrigger("Die");
        yield return Utilities.WaitForSeconds(1f);
        ObjectPoolingManager.ReturnObject(gameObject);
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
