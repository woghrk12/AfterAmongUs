using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBehaviour : MonoBehaviour
{
    protected SpriteRenderer spriteRenderer;
    protected Animator anim;

    [SerializeField] protected CharacterDamagable healthController;

    protected Coroutine attackCo;
    protected Coroutine onDamageCo = null;

    protected Region spawnRegion;

    public bool IsDie 
    {
        set 
        {
            healthController.IsDie = value;
            if (healthController.IsDie) healthController.Die();
        }
        get { return healthController.IsDie; } 
    }

    private void Awake()
    {
        spriteRenderer = GetComponentInChildren<SpriteRenderer>();
        anim = GetComponent<Animator>();
    }

    protected virtual void OnEnable()
    {
        healthController.onDamageEvent += OnDamageEvent;
        healthController.onDieEvent += OnDieEvent;
    }

    private void OnDisable()
    {
        healthController.onDamageEvent -= OnDamageEvent;
        healthController.onDieEvent -= OnDieEvent;
    }

    public virtual void SetEnemy(Region p_region)
    {
        healthController.SetHealth(false);
        spriteRenderer.enabled = false;
        spawnRegion = p_region;
        transform.position = PointManager.GetPoint(p_region.dstPoint).transform.position;
    }

    private void OnDamageEvent()
    {
        if (onDamageCo != null) onDamageCo = StartCoroutine(OnDamageCo());
    }

    private IEnumerator OnDamageCo()
    {
        spriteRenderer.color = Color.red;
        yield return new WaitForSeconds(0.05f);
        spriteRenderer.color = Color.white;

        onDamageCo = null;
    }
    
    private void OnDieEvent()
    {
        if (attackCo != null) StopCoroutine(attackCo);
        StartCoroutine(DieCo());
    }

    protected virtual IEnumerator DieCo()
    {
        gameObject.layer = (int)ELayer.ENEMYDIE;
        anim.SetTrigger("Die");

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
            healthController.OnDamage(t_damage);
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
