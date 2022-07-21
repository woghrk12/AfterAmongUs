using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Mission : MonoBehaviour
{
    private Animator anim = null;
    private BoxCollider2D hitBox = null;

    private InGameManager inGameManager = null;
    private InGameUIManager inGameUIManager = null;

    [SerializeField] private Region region = null;

    public float completeTime = 0;

    private Coroutine onDamageCo = null;
    private Coroutine alertCo = null;

    [SerializeField] private GameObject effectParent = null;
    private IEffect[] effects = null;

    [SerializeField] private CharacterDamagable healthController = null;

    [SerializeField] private SpriteRenderer coreSprite = null;

    private void Awake()
    {
        anim = GetComponent<Animator>();
        hitBox = GetComponent<BoxCollider2D>();

        effects = effectParent.GetComponentsInChildren<IEffect>();

        healthController.onDamageEvent += OnDamageEvent;
        healthController.onDieEvent += OnDieEvent;
    }

    private void Start()
    {
        inGameManager = InGameManager.instance;
        inGameUIManager = InGameUIManager.instance;

        healthController.SetHealth(false);
    }

    public void SetMission(out Region p_target)
    {
        p_target = region;
        
        hitBox.enabled = true;

        for (int i = 0; i < effects.Length; i++)
            effects[i].StartEffect();

        anim.SetBool("isActivated", true);
    }

    public IEnumerator PerformMission()
    {
        yield return inGameUIManager.TimeCheck(completeTime);

        inGameManager.EndMission(true);
    }

    private void OnDamageEvent()
    {
        if (onDamageCo == null) onDamageCo = StartCoroutine(OnDamageCo());
        if (alertCo == null) alertCo = StartCoroutine(ShowAlertTextCo());
    }

    private IEnumerator OnDamageCo()
    {
        coreSprite.color = Color.red;

        yield return new WaitForSeconds(0.05f);

        coreSprite.color = Color.white;

        onDamageCo = null;
    }

    private IEnumerator ShowAlertTextCo()
    {
        yield return InGameUIManager.AlertUnderAttack(this.name);

        alertCo = null;
    }

    private void OnDieEvent()
    {
        hitBox.enabled = false;

        anim.SetBool("isActivated", false);

        for (int i = 0; i < effects.Length; i++)
            effects[i].StopEffect();

        StartCoroutine(BrokenCoreObject());

        inGameManager.EndMission(false);
    }

    private IEnumerator BrokenCoreObject()
    {
        var t_timer = 0f;
        var t_totalTime = 1f;

        while (t_timer <= t_totalTime)
        {
            coreSprite.color = Color.Lerp(Color.white, Color.gray, t_timer / t_totalTime);
            t_timer += Time.deltaTime;
            yield return null;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("EnemyBullet"))
        {
            var t_damage = collision.GetComponent<Bullet>().damage;
            healthController.OnDamage(t_damage);
            ObjectPooling.ReturnObject(collision.gameObject);
        }
    }
}
