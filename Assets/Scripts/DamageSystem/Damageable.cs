using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Damageable : MonoBehaviour
{
    public int maxHealth = 1;

    [SerializeField] HealthBar healthBar;

    int _health;

    public int Health 
    {
        get => _health; 

        set
        {
            _health = Mathf.Clamp(value, 0, maxHealth);
            healthBar?.UpdateHealthBar(_health);

            if (_health == 0)
            {
                //da rivedere quando ci saranno le animazioni
                Die();
            }
        }
    }

    private void Start()
    {
        SetMaxHealth();
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

    public void TakeDamage(int healthToRemove)
    {
        ChangeHealth(-healthToRemove);
    }

    public void Die()
    {
        GameManager.Instance.levelMaster.Respawn();
        //gameObject.SetActive(false);
    }
}
