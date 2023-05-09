using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Damageable : MonoBehaviour
{
    public int maxHealth = 1;

    int health;


    private void Start()
    {
        health = maxHealth;
    }

    public void ChangeHealth(int healthChanged)
    {
        health += healthChanged;

        health = Mathf.Clamp(health,0,maxHealth);
    }

}
