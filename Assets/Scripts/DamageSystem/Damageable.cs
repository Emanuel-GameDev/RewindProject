using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Damageable : MonoBehaviour
{
    public int maxHealth = 1;

    [SerializeField] public HealthBar healthBar;
    [SerializeField] public bool knockable = false;

    int _health;

    [HideInInspector] public bool invincible = false;
    [SerializeField] public float invincibilitySeconds = 0.2f;

    private Damager lastDamager;

    public int Health 
    {
        get => _health; 

        set
        {
            int oldHealth= _health;
            _health = Mathf.Clamp(value, 0, maxHealth);
            healthBar?.UpdateHealthBar(_health);

            if (_health == 0)
            {

                //da rivedere quando ci saranno le animazioni
                Die();
            }
            else if(oldHealth > _health)
            {
                TakeHit();
            }
            else
            {
                TakeHeal();
            }
        }
    }

    private void TakeHeal()
    {
        OnHeal?.Invoke();
    }

    [SerializeField] UnityEvent OnDie;
    [SerializeField] UnityEvent OnHit;
    [SerializeField] UnityEvent OnHeal;


    private void Start()
    {
        SetMaxHealth();
        invincible = false;
    }

    public void SetMaxHealth()
    {
        _health = maxHealth;
        healthBar?.InitializeHealthBar(_health);
    }

    private void ChangeHealth(int healthChanged)
    {
        Health += healthChanged;
    }

    public void Heal(int healthToHeal)
    {
        ChangeHealth(healthToHeal);
    }

    public virtual void TakeDamage(int healthToRemove, Damager damager)
    {
        if (!invincible)
        {
            lastDamager = damager;
            ChangeHealth(-healthToRemove);
            if(gameObject.activeSelf)
                StartCoroutine(InvincibilitySecons(invincibilitySeconds));
        }
    }

    public void Die()
    {
        OnDie?.Invoke();
    }
    public void TakeHit()
    {
        OnHit?.Invoke();
    }

    IEnumerator InvincibilitySecons(float seconds)
    {
        invincible = true;
        yield return new WaitForSeconds(seconds);
        invincible = false;
    }

    public Damager GetLastDamager()
    {
        return lastDamager;
    }

}
