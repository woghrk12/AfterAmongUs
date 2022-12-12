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
    public UnityAction HitEvent { set; get; }
    public UnityAction DieEvent { set; get; }

    public void StartChecking() 
    {
        curHealth = maxHealth;
        hitBox.SetActive(true);
        checkCo = StartCoroutine(CheckHealth());
    }

    public void StopChecking()
    {
        hitBox.SetActive(false);

        if (checkCo is null) return;
        
        StopCoroutine(checkCo);
        checkCo = null;
    }

    private IEnumerator CheckHealth()
    {
        while (curHealth > 0) { yield return null; }
        curHealth = 0;
        DieEvent.Invoke();
        hitBox.SetActive(false);
        checkCo = null;
    }

    public void OnHit(int p_damage)
    {
        curHealth -= p_damage;
        
        if (curHealth > 0) 
        {
            HitEvent.Invoke();
            return;
        }
    }
}
