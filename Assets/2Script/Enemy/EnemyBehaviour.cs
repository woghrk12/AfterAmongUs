using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBehaviour : MonoBehaviour
{
    protected SpriteRenderer spriteRenderer;
    protected Animator anim;

    [SerializeField] protected int health;
    [SerializeField] protected int maxHealth;
    [SerializeField] protected ControlSlider healthBar;

    protected Coroutine attackCo;

    protected Region spawnRegion;

    protected bool isDie;

    private void Awake()
    {
        spriteRenderer = GetComponentInChildren<SpriteRenderer>();
        anim = GetComponent<Animator>();
    }

    protected virtual void Update()
    {
        CheckHealth();
    }

    public virtual void SetEnemy(Region p_region)
    {
        health = maxHealth;
        healthBar.SetMaxValue(maxHealth);
        healthBar.gameObject.SetActive(false);
        spriteRenderer.enabled = false;
        spawnRegion = p_region;
        transform.position = PointManager.GetPoint(p_region.dstPoint).transform.position;
    }

    public virtual void OnDamage(int p_damage)
    {
        health -= p_damage;
        
        if (!healthBar.gameObject.activeSelf) healthBar.gameObject.SetActive(true);
        if (health <= 0) health = 0;

        healthBar.SetValue(health);

        StartCoroutine(OnDamageCo());
    }

    protected virtual IEnumerator OnDamageCo()
    {
        spriteRenderer.color = Color.red;
        yield return new WaitForSeconds(0.05f);
        spriteRenderer.color = Color.white;
    }

    protected virtual void CheckHealth()
    {
        if (health > 0) return;
        if (isDie) return;

        if (attackCo != null) StopCoroutine(attackCo);
        StartCoroutine(DieCo());
    }

    public void Die()
    {
        health = 0;
    }

    protected virtual IEnumerator DieCo()
    {
        isDie = true;
        gameObject.layer = (int)ELayer.ENEMYDIE;
        anim.SetTrigger("Die");

        healthBar.gameObject.SetActive(false);

        yield return new WaitForSeconds(3f);

        gameObject.layer = (int)ELayer.ENEMY;
        ObjectPooling.ReturnObject(gameObject);
    }

    protected virtual void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Camera Collider"))
        {
            spriteRenderer.enabled = true;
        }

        if (collision.CompareTag("Bullet"))
        {
            var t_damage = collision.GetComponent<Bullet>().damage;
            OnDamage(t_damage);
            ObjectPooling.ReturnObject(collision.gameObject);
        }
    }

    protected virtual void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Camera Collider"))
        {
            spriteRenderer.enabled = false;
        }
    }
}
