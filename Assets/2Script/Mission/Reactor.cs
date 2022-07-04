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
    [SerializeField] private GameObject spriteParent;
    private SpriteRenderer[] sprites;

    [SerializeField] private ControlSlider controlSlider;
    [SerializeField] private int maxHealth;
    private int curHealth;

    private Coroutine onDamageCo;

    private void Awake()
    {
        anim = GetComponent<Animator>();
        hitBox = GetComponent<BoxCollider2D>();

        sprites = spriteParent.GetComponentsInChildren<SpriteRenderer>();
    }

    private void Start()
    {
        controlSlider.SetMaxValue(maxHealth);
        curHealth = maxHealth;
    }

    private void Update()
    {
        CheckHealth();
    }

    public void StartMission()
    {
        StartCoroutine(StartMissionCo());
    }

    private IEnumerator StartMissionCo()
    {
        hitBox.enabled = true;

        yield return ChangeLight();

        runningCo = StartCoroutine(PerformMission());
    }

    private IEnumerator ChangeLight()
    {
        yield return InGameUIManager.FadeOut();

        InGameManager.SetPlayerRegion(region);

        anim.SetBool("isActivated", true);

        InGameManager.TurnOnPointLight();

        yield return InGameUIManager.FadeIn();
    }

    private IEnumerator PerformMission()
    {
        yield return InGameUIManager.TimeCheck(completeTime);

        IsComplete = true;
    }

    public bool SuccessMission()
    {
        anim.SetTrigger("Complete");
        InGameManager.TurnOnGlobalLight();
        hitBox.enabled = false;

        return true;
    }

    public bool FailMission()
    {
        StopCoroutine(runningCo);
        runningCo = null;

        InGameManager.TurnOnGlobalLight();

        anim.SetBool("isActivated", false);

        hitBox.enabled = false;

        InGameManager.missionInProgress = null;

        StartCoroutine(InGameManager.TurnOnColorLight(new Color(1f, 0.5f, 0.5f), 3));

        return false;
    }

    private void OnDamage(int p_damage)
    {
        if (!controlSlider.gameObject.activeSelf) controlSlider.gameObject.SetActive(true);

        curHealth -= p_damage;
        controlSlider.SetValue(curHealth);
        if (onDamageCo == null) onDamageCo = StartCoroutine(OnDamageCo());
    }

    private void CheckHealth()
    {
        if (curHealth > 0) return;

        controlSlider.SetMaxValue(maxHealth);
        curHealth = maxHealth;

        controlSlider.gameObject.SetActive(false);

        FailMission();
    }

    private IEnumerator OnDamageCo()
    {
        for (int i = 0; i < sprites.Length; i++)
            sprites[i].color = Color.red;

        yield return new WaitForSeconds(0.05f);

        for (int i = 0; i < sprites.Length; i++)
            sprites[i].color = Color.white;

        onDamageCo = null;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("EnemyBullet"))
        {
            var t_damage = collision.GetComponent<Bullet>().damage;
            OnDamage(t_damage);
            ObjectPooling.ReturnObject(collision.gameObject);
        }
    }
}
