using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class CharacterDamagable : MonoBehaviour
{
    [SerializeField] private ControlSlider healthBar = null;
    [SerializeField] private int maxHealth = 0;
    private int curHealth = 0;

    private bool isDie = false;
    public bool IsDie { set { isDie = value; } get { return isDie; } }

    public event UnityAction onDamageEvent;
    public event UnityAction onDieEvent;

    private void Update()
    {
        if (curHealth <= 0) Die();
    }

    public void SetHealth()
    {
        IsDie = false;
        healthBar.SetMaxValue(maxHealth);
        curHealth = maxHealth;
    }

    public void OnDamage(int p_damage)
    {
        if (IsDie) return;

        curHealth -= p_damage;
        healthBar.SetValue(curHealth);

        if (onDamageEvent != null)
            onDamageEvent.Invoke();
    }

    private void Die()
    {
        IsDie = true;

        if (onDieEvent != null)
            onDieEvent.Invoke();
    }
}
