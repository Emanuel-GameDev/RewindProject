using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AbilityHolder : Character
{
    public Ability activeAbility;

    private EAbilityState state = EAbilityState.ready;
    private float tmpCooldown; 

    [SerializeField] KeyCode interactKey;

    private void Update()
    {
        if (activeAbility == null) return;

        switch (state)
        {
            case EAbilityState.ready:
                if (Input.GetKeyDown(interactKey))
                {
                    activeAbility.Activate(gameObject);
                    tmpCooldown = activeAbility.cooldownTime;
                    state = EAbilityState.active;
                }
                break;
            case EAbilityState.active:
                if ( activeAbility.activeTime > 0 )
                {
                    activeAbility.activeTime -= Time.deltaTime;
                }
                else
                {
                    state = EAbilityState.cooldown;
                }
                break;
            case EAbilityState.cooldown:
                if (tmpCooldown > 0)
                {
                    tmpCooldown -= Time.deltaTime;
                }
                else
                {
                    state = EAbilityState.ready;
                }
                break;
        }
    }
}
