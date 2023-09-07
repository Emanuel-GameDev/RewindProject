using System;
using System.Collections;
using System.Collections.Generic;
using System.Timers;
using UnityEditor.Sprites;
using UnityEngine;

[CreateAssetMenu(menuName = "Ability/Death")]
public class Death : Ability
{
    [SerializeField] int newDamage;
    private int oldDamage;
    private Damager damager;
    private bool deathIsActive = false;
    
    public override void Activate1(GameObject parent)
    {
        damager = parent.GetComponentInChildren<Damager>();
        if(!deathIsActive) StartOverDamage();
    }
    public override void Disactivate1(GameObject gameObject)
    {
        
    }

    private void StartOverDamage()
    {

    }

    private void SetNormalDamage()
    {
        if (damager != null)
        {
            damager.damage = oldDamage;
            deathIsActive = false;
        }
    }

    private void SetOverDamage()
    {
        if (damager != null)
        {
            oldDamage = damager.damage;
            damager.damage = newDamage;
            deathIsActive = true;
        }
    }

}
