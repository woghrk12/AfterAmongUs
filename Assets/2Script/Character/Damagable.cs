using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Damagable : MonoBehaviour
{
    [SerializeField] private GameObject hitBox = null;
    [SerializeField] private ControlSlider healthBar = null;
    [SerializeField] private int maxHealth = 0;
    private int curHealth = 0;
    private Coroutine checkCo = null;

    public ControlSlider HealthBar { set { healthBar = value; } get { return healthBar; } }
    public UnityAction HitEvent { set; get; }
    public UnityAction DieEvent { set; get; }

    public void StartChecking(bool p_isShow = false) 
    {
        curHealth = maxHealth;
        healthBar.SetMaxValue(maxHealth);
        if (!p_isShow) healthBar.gameObject.SetActive(false);
        hitBox.SetActive(true);
        checkCo = StartCoroutine(CheckHealth(p_isShow));
    }

    public void StopChecking()
    {
        hitBox.SetActive(false);

        if (checkCo is null) return;
        
        StopCoroutine(checkCo);
        checkCo = null;
    }

    private IEnumerator CheckHealth(bool p_isShow)
    {
        while (curHealth > 0) { yield return null; }
        curHealth = 0;

        if (!p_isShow) healthBar.gameObject.SetActive(false);
        else healthBar.SetValue(curHealth);
        
        DieEvent.Invoke();
        hitBox.SetActive(false);
        checkCo = null;
    }

    public void OnHit(int p_damage)
    {
        curHealth -= p_damage;
        
        if (curHealth > 0) 
        {
            if (!healthBar.gameObject.activeSelf) healthBar.gameObject.SetActive(true);
            healthBar.SetValue(curHealth);
            HitEvent.Invoke();
            return;
        }
    }
}
