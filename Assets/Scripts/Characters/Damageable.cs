using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Damageable : MonoBehaviour
{
    public int maxHealtPoint { get; private set; } = 3;
    public int healtPoint { get; private set; } = 3;

    public void SetMaxHP(int HP)
    {
        maxHealtPoint = HP;
    }

    public void TakeDamage(int damage)
    {
        healtPoint -= damage;

        if (healtPoint <= 0)
        {
            healtPoint = 0;
            HandleDeath();
        }

        Debug.Log(gameObject.name + " take " + damage + " damage! \n Remaining HP: " + healtPoint);

    }

    public void Heal(int heal)
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

    public void Initilize(int maxHP) 
    { 
        SetMaxHP(maxHP);
        healtPoint = maxHP;
    }
}
