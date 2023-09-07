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
    private float elapsed;
    private Damager damager;
    
    public override void Activate1(GameObject parent)
    {
        damager = parent.GetComponentInChildren<Damager>();
        if(damager != null)
        {
            oldDamage = damager.damage;
            damager.damage = newDamage;
            isActive = true;
            elapsed = 0;
        }
    }

    public override void Disactivate1(GameObject gameObject)
    {
        damager = gameObject.GetComponentInChildren<Damager>();
        SetNormalDamage();
    }

    private void SetNormalDamage()
    {
        if (damager != null)
        {
            damager.damage = oldDamage;
            isActive = false;
        }
    }

    public override void UpdateAbility()
    {
        if (isActive)
        {
            elapsed += Time.deltaTime;
            if (elapsed > cooldownTime)
            {
                SetNormalDamage();
            }
        }
    }
}
