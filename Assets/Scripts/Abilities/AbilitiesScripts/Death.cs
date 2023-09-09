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
    [SerializeField] Material newMaterial;
    private Material oldMaterial;
    private int oldDamage;
    private Damager damager;
    private SpriteRenderer spriteRenderer;
    private float elapsedTime = 0;
    
    public override void Activate1(GameObject parent)
    {
        damager = parent.GetComponentInChildren<Damager>();
        spriteRenderer = parent.GetComponentInChildren<SpriteRenderer>();
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
            SetNormalDamageColor();
            damager.damage = oldDamage;
            isActive = false;
        }
    }

    private void SetNormalDamageColor()
    {
        if (spriteRenderer != null)
        {
            spriteRenderer.material = oldMaterial;
        }
    }

    private void SetOverDamage()
    {
        if (damager != null)
        {
            SetOverDamageColor();
            oldDamage = damager.damage;
            damager.damage = newDamage;
            isActive = true;
            elapsedTime = 0;
        }
    }

    private void SetOverDamageColor()
    {
        if(spriteRenderer != null)
        {
            oldMaterial = spriteRenderer.material;
            spriteRenderer.material = newMaterial;
        }
    }

    public override void Pick(Character picker)
    {
        base.Pick(picker);
        isActive = false;
    }

}
