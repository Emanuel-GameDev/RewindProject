using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Damageable : MonoBehaviour
{
    public float maxHealtPoint { get; private set; } = 3;
    public float healtPoint { get; private set; } = 3;

    public void SetMaxHP(float HP)
    {
        maxHealtPoint = HP;
    }

    public void TakeDamage(float damage)
    {
        healtPoint -= damage;

        if (healtPoint <= 0)
        {
            healtPoint = 0;
            HandleDeath();
        }

        Debug.Log(gameObject.name + " take " + damage + " damage! \n Remaining HP: " + healtPoint);

    }

    public void Heal(float heal)
    {
        healtPoint += heal;
        if (healtPoint > maxHealtPoint) 
        { 
            healtPoint = maxHealtPoint;
        }
    }

    private void HandleDeath()
    {
        Debug.Log(gameObject.name + " is dead!");
    }

    public void Initilize(float maxHP) 
    { 
        SetMaxHP(maxHP);
        healtPoint = maxHP;
    }
}
