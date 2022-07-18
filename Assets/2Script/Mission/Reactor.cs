using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Reactor : MonoBehaviour, IMission
{
    private Animator anim;
    private BoxCollider2D hitBox;

    [SerializeField] private Region region;

    [SerializeField] private float completeTime;
    private bool isComplete = false;
    private bool IsComplete
    {
        set
        {
            isComplete = value;
            if (value) SuccessMission();
            else FailMission();
        }
    }

    private Coroutine runningCo;
    [SerializeField] private GameObject effectParent;
    private IEffect[] effects;

    [SerializeField] private CharacterDamagable healthController;

    private Coroutine onDamageCo;
    private Coroutine alertCo;
    [SerializeField] private SpriteRenderer coreSprite;

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
        healthController.SetHealth(false);
    }

    public void StartMission()
    {
        StartCoroutine(StartMissionCo());
    }

    private IEnumerator StartMissionCo()
    {
        hitBox.enabled = true;

        yield return InGameUIManager.FadeOut();

        for (int i = 0; i < effects.Length; i++)
            effects[i].StartEffect();

        InGameManager.SetPlayerRegion(region);

        anim.SetBool("isActivated", true);

        //InGameManager.TurnOnPointLight();

        yield return InGameUIManager.FadeIn();

        runningCo = StartCoroutine(PerformMission());
    }

    private IEnumerator PerformMission()
    {
        yield return InGameUIManager.TimeCheck(completeTime);

        IsComplete = true;
    }

    public bool SuccessMission()
    {
        InGameManager.TurnOnGlobalLight();
        hitBox.enabled = false;

        anim.SetTrigger("Complete");

        InGameManager.missionInProgress = null;
        InGameManager.instance.NumCompleteMission++;

        StartCoroutine(InGameManager.TurnOnColorLight(new Color(0.6f, 1f, 0.6f), 1));

        return true;
    }

    public bool FailMission()
    {
        StopCoroutine(runningCo);
        runningCo = null;

        InGameManager.TurnOnGlobalLight();
        hitBox.enabled = false;

        anim.SetBool("isActivated", false);

        for (int i = 0; i < effects.Length; i++)
            effects[i].StopEffect();

        InGameManager.missionInProgress = null;
        InGameManager.instance.NumFailMission++;

        StartCoroutine(InGameManager.TurnOnColorLight(new Color(1f, 0.5f, 0.5f), 3));

        return false;
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

    private void OnDieEvent() => FailMission();

    private IEnumerator ShowAlertTextCo()
    {
        yield return InGameUIManager.AlertUnderAttack(this.name);

        alertCo = null;
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