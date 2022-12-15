using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyPortal : MonoBehaviour
{
    [SerializeField] private Animator anim = null;
    [SerializeField] private SpriteRenderer sprite = null;

    [SerializeField] private Damagable hitController = null;

    [SerializeField] private GameObject enemyPrefab = null;

    private void OnEnable()
    {
        hitController.HitEvent += OnHit;
        hitController.DieEvent += OnDie;

        hitController.StartChecking();
    }

    private void OnDisable()
    {
        hitController.HitEvent -= OnHit;
        hitController.DieEvent -= OnDie;
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.F1))
            SpawnEnemy();
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
