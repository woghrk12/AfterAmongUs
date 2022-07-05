using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LowerEngine : MonoBehaviour, IMission
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

    [SerializeField] private ControlSlider controlSlider;
    [SerializeField] private int maxHealth;
    private int curHealth;

    private Coroutine onDamageCo;
    [SerializeField] private SpriteRenderer coreSprite;

    private void Awake()
    {
        anim = GetComponent<Animator>();
        hitBox = GetComponent<BoxCollider2D>();
    }

    private void Start()
    {
        controlSlider.SetMaxValue(maxHealth);
        curHealth = maxHealth;
    }

    private void Update()
    {
        CheckHealth();

        if (Input.GetKeyDown(KeyCode.F4))
            StartCoroutine(BrokenCoreObject());
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

        //InGameManager.TurnOnPointLight();

        yield return InGameUIManager.FadeIn();
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
        controlSlider.gameObject.SetActive(false);

        InGameManager.missionInProgress = null;
        InGameManager.instance.NumCompleteMission++;

        StartCoroutine(InGameManager.TurnOnColorLight(new Color(0.6f, 1f, 0.6f), 1));

        for (int i = 0; i < InGameManager.enemys.Count; i++)
            InGameManager.enemys[i].Die();

        return true;
    }

    public bool FailMission()
    {
        StopCoroutine(runningCo);
        runningCo = null;

        InGameManager.TurnOnGlobalLight();
        hitBox.enabled = false;

        anim.SetBool("isActivated", false);
        controlSlider.gameObject.SetActive(false);

        StartCoroutine(BrokenCoreObject());

        InGameManager.missionInProgress = null;
        InGameManager.instance.NumFailMission++;

        StartCoroutine(InGameManager.TurnOnColorLight(new Color(1f, 0.5f, 0.5f), 3));

        for (int i = 0; i < InGameManager.enemys.Count; i++)
            InGameManager.enemys[i].Die();

        return false;
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
    private void OnDamage(int p_damage)
    {
        if (!controlSlider.gameObject.activeSelf) controlSlider.gameObject.SetActive(true);

        curHealth -= p_damage;
        controlSlider.SetValue(curHealth);
        if (onDamageCo == null) onDamageCo = StartCoroutine(OnDamageCo());
    }

    private IEnumerator OnDamageCo()
    {
        coreSprite.color = Color.red;

        yield return new WaitForSeconds(0.05f);

        coreSprite.color = Color.white;

        onDamageCo = null;
    }

    private void CheckHealth()
    {
        if (curHealth > 0) return;

        controlSlider.SetMaxValue(maxHealth);
        curHealth = maxHealth;

        controlSlider.gameObject.SetActive(false);

        FailMission();
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


