using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyPortal : EnemyBehaviour
{
    public MiniMapObject miniMapObject;

    private void Awake()
    {
        spriteRenderer = GetComponentInChildren<SpriteRenderer>();
        anim = GetComponent<Animator>();
    }

    protected override IEnumerator DieCo()
    {
        MiniMapManager.ReturnObject(miniMapObject);

        return base.DieCo();
    }

    public override void SetEnemy(Region p_region)
    {
        base.SetEnemy(p_region);

        attackCo = StartCoroutine(SpawnEnemyCo());
    }

    private IEnumerator SpawnEnemyCo()
    {
        while (!healthController.IsDie)
        {
            var t_randomTime = Random.Range(5f, 8f);

            yield return new WaitForSeconds(t_randomTime);
            
            var t_enemy = ObjectPooling.SpawnObject("EnemyNormal", transform.position, Quaternion.identity).GetComponent<EnemyNormal>();
            t_enemy.SetEnemy(spawnRegion);
        }
    }
}
