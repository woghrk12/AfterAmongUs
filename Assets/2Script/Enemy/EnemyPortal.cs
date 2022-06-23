using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyPortal : MonoBehaviour
{
    private SpriteRenderer spriteRenderer;

    [SerializeField] private int health;
    [SerializeField] private int maxHealth;
    [SerializeField] private ControlSlider healthBar;

    public Region spawnRegion;

    private void Awake()
    {
        spriteRenderer = GetComponentInChildren<SpriteRenderer>();
    }

    private void OnEnable()
    {
        health = maxHealth;
        healthBar.SetMaxValue(maxHealth);
    }

    public void SpawnEnemy() => StartCoroutine(SpawnEnemyCo());

    private IEnumerator SpawnEnemyCo()
    {
        while (health > 0)
        {
            var t_randomTime = Random.Range(5f, 8f);

            Debug.Log(t_randomTime);
            yield return new WaitForSeconds(t_randomTime);
            
            var t_enemy = ObjectPooling.SpawnObject("Enemy", transform.position, Quaternion.identity);
            t_enemy.GetComponent<EnemyBehaviour>().SetEnemy(spawnRegion);
        }
    }
}
