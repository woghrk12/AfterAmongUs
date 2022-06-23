using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyPortal : EnemyBehaviour
{ 
    private void Awake()
    {
        spriteRenderer = GetComponentInChildren<SpriteRenderer>();
        anim = GetComponent<Animator>();
    }

    public void SpawnEnemy()
    {
        attackCo = StartCoroutine(SpawnEnemyCo());
    }

    private IEnumerator SpawnEnemyCo()
    {
        while (health > 0)
        {
            var t_randomTime = Random.Range(5f, 8f);

            Debug.Log(t_randomTime);
            yield return new WaitForSeconds(t_randomTime);
            
            var t_enemy = ObjectPooling.SpawnObject("EnemyNormal", transform.position, Quaternion.identity);
            t_enemy.GetComponent<EnemyNormal>().SetEnemy(spawnRegion);
        }
    }
}
