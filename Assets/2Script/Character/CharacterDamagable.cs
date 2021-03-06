using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class CharacterDamagable : MonoBehaviour
{
    [SerializeField] private ControlSlider healthBar = null;
    [SerializeField] private int maxHealth = 0;
    private int curHealth = 0;

    [SerializeField] private bool isPlayer = false;
    private bool isDie = false;
    public bool IsDie { set { isDie = value; } get { return isDie; } }

    public event UnityAction onDamageEvent;
    public event UnityAction onDieEvent;

    private void Update()
    {
        if (curHealth <= 0 && !IsDie) Die(isPlayer);
    }

    public void SetHealth(bool p_isPlayer)
    {
        IsDie = false;
        healthBar.SetMaxValue(maxHealth);
        curHealth = maxHealth;

        if (!p_isPlayer) healthBar.gameObject.SetActive(false);
    }

    public void OnDamage(int p_damage)
    {
        if (IsDie) return;

        if (!healthBar.gameObject.activeSelf) healthBar.gameObject.SetActive(true);

        curHealth -= p_damage;
        healthBar.SetValue(curHealth);

        if (onDamageEvent != null)
            onDamageEvent.Invoke();
    }

    public void Die(bool p_isOn)
    {
        IsDie = true;

        healthBar.gameObject.SetActive(p_isOn);

        if (onDieEvent != null)
            onDieEvent.Invoke();
    }

    public void Cure(int p_value)
    {
        if (IsDie) return;

        curHealth = curHealth + p_value > maxHealth ? maxHealth : curHealth + p_value;
        healthBar.SetValue(curHealth);
    }
}
