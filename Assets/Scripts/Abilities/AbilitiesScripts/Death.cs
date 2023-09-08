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
    private float elapsedTime = 0;
    
    public override void Activate1(GameObject parent)
    {
        damager = parent.GetComponentInChildren<Damager>();
        if(!isActive) SetOverDamage();
    }

    public override void UpdateAbility()
    {
        if (isActive)
        {
            elapsedTime += Time.deltaTime;
            if(elapsedTime > cooldownTime)
            {
                SetNormalDamage();
            }
        }
    }

    private void SetNormalDamage()
    {
        if (damager != null)
        {
            damager.damage = oldDamage;
            isActive = false;
        }
    }

    private void SetOverDamage()
    {
        if (damager != null)
        {
            oldDamage = damager.damage;
            damager.damage = newDamage;
            isActive = true;
            elapsedTime = 0;
        }
    }

    public override void Pick(Character picker)
    {
        base.Pick(picker);
        isActive = false;
    }

}
